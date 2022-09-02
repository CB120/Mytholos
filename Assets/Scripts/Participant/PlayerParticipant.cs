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
    private void Awake()
    {
        base.Awake(); //Do not remove!
    }

    private void OnSelectLeft(InputValue value)
    {
        if (selectedMythIndex == -1 && value.Get<float>() > 0.5)
            selectedMythIndex = mythsInPlay[0];

        if (selectedMythIndex == mythsInPlay[0] && value.Get<float>() < 0.5)
            selectedMythIndex = -1;
    }
    
    private void OnSelectRight(InputValue value)
    {
        if (selectedMythIndex == -1 && value.Get<float>() > 0.5)
            selectedMythIndex = mythsInPlay[1];

        if (selectedMythIndex == mythsInPlay[1] && value.Get<float>() < 0.5)
            selectedMythIndex = -1;
    }

    private void OnUseAbilityWest(InputValue value)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new AbilityCommand();
    }

    private void OnMoveStrategyUp(InputValue value)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Stay);
    }

    private void OnMoveStrategyDown(InputValue value)
    {
        if (SelectedMyth)
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
    } 
}
