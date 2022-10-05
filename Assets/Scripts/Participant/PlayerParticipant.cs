using System;
using System.Collections;
using System.Linq;
using Commands;
using Myths;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerParticipant : Participant
{
    //Events
    public UnityEvent<int> SelectMyth = new();
    public UnityEvent<SO_Ability> SelectAbility = new();

    //Properties


    //Variables
    //int[] mythsInPlay = { 0, 1 }; //Stores indexes of Myth references in party[] corresponding to each controller 'side'/shoulder button
                                  // L  R   | mythsInPlay[0] = Left monster = party[mythsInPlay[0]] | opposite for Right monster

    //References
    private int selectedMythIndex = 0;

    private Myth MythInPlay => ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(selectedMythIndex);
    // TODO: Should be cached for performance
    private MythCommandHandler SelectedMythCommandHandler => MythInPlay.GetComponent<MythCommandHandler>();

    // Menu references
    public UIMenuNodeGraph currentMenuGraph;
    Coroutine cancelCoroutine;

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
        if (!context.performed) return;

        SelectedMythCommandHandler.Command = new DodgeCommand();

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
        if (SelectedMythCommandHandler.Command is not ManualMoveCommand)
            SelectedMythCommandHandler.Command = new ManualMoveCommand();

        if (SelectedMythCommandHandler.Command is ManualMoveCommand manualMoveCommand)
        {
            manualMoveCommand.input = context.ReadValue<Vector2>();
        }
    }
    private void UseAbility(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
    {
        if (!context.performed) return;

        if (!MythInPlay) return;

        var ability = abilityAccessor(MythInPlay);

        // TODO: I don't think this is the right place for this check
        if (MythInPlay.Stamina.Value < ability.stamina) return;

        if (!ability.isRanged)
        {
            SelectedMythCommandHandler.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
            Debug.Log("Close range attack");
            return;
        }

        SelectedMythCommandHandler.Command = new AbilityCommand(ability);

        SelectAbility.Invoke(ability);
    }

    public void SwitchLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Left trigger to switch has been activated");
    }

    public void SwitchRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Right trigger to switch has been activated");
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
}
