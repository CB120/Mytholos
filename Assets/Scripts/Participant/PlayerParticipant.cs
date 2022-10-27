using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Myths;
using StateMachines;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerParticipant : Participant
{
    // Technically a cyclic dependency, but more effort than it's worth to solve
    [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
    
    //Events
    public UnityEvent<int> SelectMyth = new();
    public UnityEvent<SO_Ability> SelectAbility = new();
    [NonSerialized] public UnityEvent<PlayerParticipant> mythInPlayChanged = new();
    [NonSerialized] public UnityEvent<PlayerParticipant> pauseRequested = new();
    [NonSerialized] public UnityEvent<PlayerParticipant> resumeRequested = new();
    public UnityEvent<bool> FaceButtonNorth = new();
    public UnityEvent<bool> FaceButtonWest = new();
    public UnityEvent<bool> FaceButtonSouth = new();
    public UnityEvent<bool> FaceButtonEast = new();

    //Properties
    private bool isAvailableToSwap = true;
    private bool isAvailableToDodge = true;
    [SerializeField] private float swappingCooldown = 1f;
    [SerializeField] private float dodgeCooldown = 2.25f;
    [SerializeField] private PlayerInput playerInput;

    public PlayerInput PlayerInput => playerInput;
    
    //Variables
    //int[] mythsInPlay = { 0, 1 }; //Stores indexes of Myth references in party[] corresponding to each controller 'side'/shoulder button
    // L  R   | mythsInPlay[0] = Left monster = party[mythsInPlay[0]] | opposite for Right monster

    //References
    public Myth MythInPlay
    {
        get => mythInPlay;
        private set
        {
            if (mythInPlay != null)
            {
                mythInPlay.SetAnimatorTrigger("Reset");
                StartCoroutine(DisableSwappedOutMyth(value));
            }
            else // This was added after commenting the following code (and only(?) gets called on scene start)
            {
                mythInPlay = value;

                if (mythInPlay != null)
                {
                    mythInPlay.gameObject.SetActive(true);
                    mythInPlay.SetAnimatorTrigger("SwapIn");
                }

                mythInPlayChanged.Invoke(this);
            }
        }
    }

    IEnumerator DisableSwappedOutMyth(Myth newMyth) // Need to wait a frame for the animator to return to a neutral pose, else when reenabled, will be incorrect
    {
        yield return new WaitForSeconds(0);
        mythInPlay.gameObject.SetActive(false);

        // Most of the following is duplicate code (beside the transform inheritance)
        Vector3 position = MythInPlay.transform.position;
        Quaternion rotation = MythInPlay.transform.rotation;
        mythInPlay = newMyth;

        if (mythInPlay != null)
        {
            mythInPlay.gameObject.SetActive(true);
            mythInPlay.SetAnimatorTrigger("SwapIn");
            MythInPlay.transform.position = position;
            MythInPlay.transform.rotation = rotation;
        }

        mythInPlayChanged.Invoke(this);
    }

    // TODO: Should be cached for performance
    private MythCommandHandler SelectedMythCommandHandler => MythInPlay.GetComponent<MythCommandHandler>();

    private List<Myth> mythsInReserve = new();
    private List<Myth> myths = new();

    // Menu references
    public UIMenuNodeGraph currentMenuGraph;
    public UIMenuNodeGraph currentShoulderMenuGraph; // Alternate menu that you navigate using L/R
    Coroutine cancelCoroutine;
    private Myth mythInPlay;

    private void OnEnable()
    {
        playerParticipantRuntimeSet.Add(this);
    }

    private void OnDisable()
    {
        playerParticipantRuntimeSet.Remove(this);
    }

    private InputActionMap oldInputActionMap;
    
    // TODO: Disabling twice will cause problems
    public void DisablePlayerInput(float timeToWait = 0)
    {
        oldInputActionMap = PlayerInput.currentActionMap;
        PlayerInput.currentActionMap = null;
        if (timeToWait > 0)
            Invoke("EnablePlayerInput", timeToWait);
    }

    public void EnablePlayerInput()
    {
        PlayerInput.currentActionMap = oldInputActionMap;
    }

    /*** In-Game Input events ***/
    #region In-game Input Events

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        FaceButtonNorth.Invoke(context.performed);
        UseAbility(context, myth => myth.NorthAbility);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        FaceButtonEast.Invoke(context.performed);

        if (!isAvailableToDodge) return;
        if (!context.performed) return;

        if (SelectedMythCommandHandler.LastCommand is MoveCommand moveCommand)
        {
            SelectedMythCommandHandler.PushCommand(new DodgeCommand(moveCommand.input));   // Dodge in the input direction if the left stick is currently in use
            StartDodgeCooldown();
        }
        else
        {
            Vector3 forwardVector = mythInPlay.transform.rotation * Vector3.forward;
            SelectedMythCommandHandler.PushCommand(new DodgeCommand(new Vector2(forwardVector.x, forwardVector.z).normalized));    // Else dodge in direction myth is facing
            StartDodgeCooldown();
        }
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        FaceButtonSouth.Invoke(context.performed);
        UseAbility(context, myth => myth.SouthAbility);
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        FaceButtonWest.Invoke(context.performed);
        UseAbility(context, myth => myth.WestAbility);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // TODO: Not sure if this logic should be here or in MoveCommandReceived
        if (SelectedMythCommandHandler.CurrentCommand is not MoveCommand moveCommand)
            SelectedMythCommandHandler.PushCommand(new MoveCommand(context.ReadValue<Vector2>()));
        else
            moveCommand.input = context.ReadValue<Vector2>();
    }
    
    private void UseAbility(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
    {
        if (!context.performed) return;

        var ability = abilityAccessor(MythInPlay);

        // TODO: I don't think this is the right place for this check
        if (MythInPlay.Stamina.Value < ability.staminaCost)
        {
            UISFXManager.PlaySound("Ability Denied " + partyIndex);
            return;
        }

        SelectedMythCommandHandler.PushCommand(new AbilityCommand(ability));

        SelectAbility.Invoke(ability);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        pauseRequested.Invoke(this);
    }

    public void SwitchLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        SwapCommandInDirection(-1);
    }

    public void SwitchRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        SwapCommandInDirection(1);
    }

    #endregion

    private void SwapCommandInDirection(int direction)
    {
        if (!isAvailableToSwap) return;
        
        if (MythInPlay.Health.Value <= 0) return;

        var mythToSwapTo = FindMythToSwapTo(direction);

        if (mythToSwapTo == null) return;
        
        PushSwapCommand(mythToSwapTo);
    }

    private void PushSwapCommand(Myth myth)
    {
        SelectedMythCommandHandler.PushCommand(new SwapCommand(myth, this));
    }

    public void SwapFromCommand(SwapCommand swapCommand)
    {
        MythInPlay = swapCommand.mythToSwapIn;
        StartSwapCooldown();
    }

    public void SwapInDirection(int preferredDirection)
    {
        var mythToSwapTo = FindMythToSwapTo(preferredDirection);

        if (mythToSwapTo == null) return;

        MythInPlay = mythToSwapTo;
        StartSwapCooldown();
    }

    private Myth FindMythToSwapTo(int preferredDirection)
    {
        int currentMythIndex = myths.IndexOf(mythInPlay);
        
        // Try to swap in specified direction
        int nextIndex = (currentMythIndex + preferredDirection) % myths.Count;
        if (nextIndex < 0) nextIndex = myths.Count - 1;

        if (CanMythBeSwappedTo(nextIndex))
            return myths[nextIndex];
        
        // Try swap in other direction
        nextIndex = (currentMythIndex - preferredDirection) % myths.Count;
        if (nextIndex < 0) nextIndex = myths.Count - 1;

        if (CanMythBeSwappedTo(nextIndex))
            return myths[nextIndex];

        return null;
    }

    private bool CanMythBeSwappedTo(int mythIndex)
    {
        return myths[mythIndex].Health.Value > 0;
    }

    /*** UI Input events ***/
    #region UI Input Events
    public void NavigateUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input UP. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Up, partyIndex, true);
    }
    public void NavigateDown(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input DOWN. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Down, partyIndex, true);
    }

    public void NavigateRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input RIGHT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);

        // If it works it works
        if (!context.performed) return;
        if (currentShoulderMenuGraph == null) return;
        currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);
    }

    public void NavigateLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input LEFT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);

        // If it works it works
        if (!context.performed) return;
        if (currentShoulderMenuGraph == null) return;
        currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);
    }

    public void NavigateRightShoulder(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentShoulderMenuGraph == null) return;
        currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);
    }

    public void NavigateLeftShoulder(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentShoulderMenuGraph == null) return;
        currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);
    }

    public void AssignNorth(InputAction.CallbackContext context)
    {
        FaceButtonNorth.Invoke(context.performed);

        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.North, partyIndex);
    }
    public void AssignWest(InputAction.CallbackContext context)
    {
        FaceButtonWest.Invoke(context.performed);

        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.West, partyIndex);
    }

    public void AssignSouth(InputAction.CallbackContext context)
    {
        FaceButtonSouth.Invoke(context.performed);

        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.South, partyIndex);
    }

    public void Submit(InputAction.CallbackContext context)
    {
        //print("Submit " + (context.performed ? "pressed!" : "released!"));

        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.Submit, partyIndex);
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        FaceButtonEast.Invoke(context.performed); // Not accurate on keyboard

        //print("Cancel " + (context.performed ? "pressed!" : "released!"));

        if (!context.performed)
        {
            if (cancelCoroutine != null)
                StopCoroutine(cancelCoroutine);
            return;
        }

        if (currentMenuGraph == null) return;

        currentMenuGraph.ParseAction(UIMenuNode.Action.Cancel, partyIndex);

        if (cancelCoroutine != null)
            StopCoroutine(cancelCoroutine);
        cancelCoroutine = StartCoroutine(HoldCancel(0.65f));
    }

    IEnumerator HoldCancel(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        if (currentMenuGraph != null)
            currentMenuGraph.ParseAction(UIMenuNode.Action.HoldCancel, partyIndex);
    }

    public void StartButton(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.Start, partyIndex);
    }

    public void Resume(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        resumeRequested.Invoke(this);
    }

    #endregion

    /*** Swapping ***/
    #region Swapping
    // TODO: How is this no longer being used?
    public void SwapReserveAtIndex(int index)
    {
        if (!isAvailableToSwap) return;
        if (mythsInReserve[index].Health.Value == 0) return;
        StartSwapCooldown();
        
        var position = MythInPlay.transform.position;
        (MythInPlay, mythsInReserve[index]) = (mythsInReserve[index], MythInPlay);
   

        MythInPlay.transform.position = position;
    }

    



    private void StartSwapCooldown()
    {
        isAvailableToSwap = false;
           Invoke("EndSwapCooldown", swappingCooldown);
    }

    private void StartDodgeCooldown()
    {
        dodgeCooldown = dodgeCooldown - mythInPlay.walkSpeed;
        isAvailableToDodge = false;
            Invoke("EndDodgeCooldown", dodgeCooldown);
    }

    private void EndSwapCooldown()
    {
        isAvailableToSwap = true;
    }

    private void EndDodgeCooldown()
    {
        isAvailableToDodge = true;
    }

    #endregion
    public void Initialise()
    {
        MythInPlay = ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(0);
        myths = ParticipantData.partyData[partyIndex].myths.ToList();

        mythsInReserve = ParticipantData.partyData[partyIndex].myths.ToList();

        if (MythInPlay != null)
            mythsInReserve.Remove(MythInPlay);
    }
}
