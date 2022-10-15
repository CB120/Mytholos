using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Myths;
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

    //Properties
    private bool isAvailableToSwap = true;
    private bool isAvailableToDodge = true;
    [SerializeField] private float swappingCooldown = 2f;
    [SerializeField] private float dodgeCooldown = 2.1f;

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
                mythInPlay.gameObject.SetActive(false);
            
            mythInPlay = value;
            
            if (mythInPlay != null)
                mythInPlay.gameObject.SetActive(true);
            
            mythInPlayChanged.Invoke(this);
        }
    }

    // TODO: Should be cached for performance
    private MythCommandHandler SelectedMythCommandHandler => MythInPlay.GetComponent<MythCommandHandler>();

    private List<Myth> mythsInReserve = new();

    // Menu references
    public UIMenuNodeGraph currentMenuGraph;
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

    public void DisablePlayerInput(float timeToWait)
    {
        PlayerInput input = GetComponent<PlayerInput>();
        if (input)
        {
            input.notificationBehavior = PlayerNotifications.SendMessages;
            Invoke("EnablePlayerInput", timeToWait);
        }    
    }

    void EnablePlayerInput()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        if (input)
        {
            input.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        }
    }

    /*** In-Game Input events ***/
    #region In-game Input Events

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.northAbility);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        if (!isAvailableToDodge) return;
        if (!context.performed) return;

        if (SelectedMythCommandHandler.Command is MoveCommand moveCommand)
        {
            SelectedMythCommandHandler.Command = new DodgeCommand(moveCommand.input);   // Dodge in the input direction if the left stick is currently in use
            StartDodgeCooldown();
        }
        else
        {
            Vector3 forwardVector = mythInPlay.transform.rotation * Vector3.forward;
            SelectedMythCommandHandler.Command = new DodgeCommand(new Vector2(forwardVector.x, forwardVector.z).normalized);    // Else dodge in direction myth is facing
            StartDodgeCooldown();
        }
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.southAbility);
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.westAbility);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (SelectedMythCommandHandler.Command is not MoveCommand)
            SelectedMythCommandHandler.Command = new MoveCommand();

        if (SelectedMythCommandHandler.Command is MoveCommand moveCommand)
        {
            moveCommand.input = context.ReadValue<Vector2>();
        }
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

        SelectedMythCommandHandler.Command = new AbilityCommand(ability);

        SelectAbility.Invoke(ability);
    }

    public void SwitchLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        SwapReserveAtIndex(0);
    }

    public void SwitchRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        SwapReserveAtIndex(1);
    }

    #endregion


    /*** UI Input events ***/
    #region UI Input Events
    public void NavigateUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input UP. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Up, partyIndex);
    }
    public void NavigateDown(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input DOWN. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Down, partyIndex);
    }

    public void NavigateRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input RIGHT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex);
    }

    public void NavigateLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        //print("Player " + partyIndex + " just input LEFT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
        currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex);
    }

    public void AssignNorth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.North, partyIndex);
    }
    public void AssignWest(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.West, partyIndex);
    }

    public void AssignSouth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.South, partyIndex);
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentMenuGraph == null) return;
        currentMenuGraph.ParseAction(UIMenuNode.Action.Submit, partyIndex);
    }

    public void Cancel(InputAction.CallbackContext context)
    {
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

    #endregion

    /*** Swapping ***/
    #region Swapping
    private void SwapReserveAtIndex(int index)
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
        
        mythsInReserve = ParticipantData.partyData[partyIndex].myths.ToList();

        if (MythInPlay != null)
            mythsInReserve.Remove(MythInPlay);
    }
}
