using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Myths;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParticipant : Participant
{
    //Properties


    //Variables
    int[] mythsInPlay = { 0, 1 }; //Stores indexes of Myth references in party[] corresponding to each controller 'side'/shoulder button
                                  // L  R   | mythsInPlay[0] = Left monster = party[mythsInPlay[0]] | opposite for Right monster


    //References

    private int selectedMythIndex = -1;

    private Myth SelectedMyth => debugParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(selectedMythIndex);

    //Engine-called
    protected override void Awake()
    {
        base.Awake(); //Do not remove!

        // TODO: This is some temporary jank to get manual movement input down to the myths.
        var playerInput = GetComponent<PlayerInput>();

        if (playerInput == null) return;

        var moveEventID = playerInput.actions.FindAction("Move").id.ToString();

        var moveEvent = playerInput.actionEvents.FirstOrDefault(e => e.actionId == moveEventID);

        if (moveEvent == null) return;
        
        foreach (var myth in debugParticipantData.partyData[partyIndex].myths)
        {
            moveEvent.AddListener(myth.GetComponent<ManualMovementStyle>().Move);
        }
    }

    private void SelectLeft(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
            selectedMythIndex = mythsInPlay[0];

        if (selectedMythIndex == mythsInPlay[0] && context.canceled)
            selectedMythIndex = -1;
    }
    
    private void SelectRight(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
            selectedMythIndex = mythsInPlay[1];

        if (selectedMythIndex == mythsInPlay[1] && context.canceled)
            selectedMythIndex = -1;
    }

    private void UseAbilityWest(InputAction.CallbackContext context)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new AbilityCommand();
    }

    private void MoveStrategyUp(InputAction.CallbackContext context)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Stay);
    }

    private void MoveStrategyDown(InputAction.CallbackContext context)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
    } 
}
