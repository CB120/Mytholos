using System;
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
    int[] mythsInPlay = { 0, 1 }; //Stores indexes of Myth references in party[] corresponding to each controller 'side'/shoulder button
                                  // L  R   | mythsInPlay[0] = Left monster = party[mythsInPlay[0]] | opposite for Right monster

    //References

    private int selectedMythIndex = -1;
    private bool EnemySwitch = false; 
    private Myth SelectedMyth1 => ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(0);
    private Myth SelectedMyth2 => ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(1);
    // TODO: Should be cached for performance
    private MythCommandHandler SelectedMythCommandHandler1 => SelectedMyth1.GetComponent<MythCommandHandler>();
    private MythCommandHandler SelectedMythCommandHandler2 => SelectedMyth2.GetComponent<MythCommandHandler>();


    private void CancelManualMovement()
    {
        if (SelectedMythCommandHandler1.Command is ManualMoveCommand manualMoveCommand)
            manualMoveCommand.input = Vector2.zero;
        else if (SelectedMythCommandHandler2.Command is ManualMoveCommand manualMoveCommand2)
                manualMoveCommand2.input = Vector2.zero;
    }

    public void UseRightAbilityNorth(InputAction.CallbackContext context)
    {
        UseAbility2(context, myth => myth.northAbility);
    }

    public void UseRightAbilityEast(InputAction.CallbackContext context)
    {
        //if (!context.performed) return;
        //CancelManualMovement();
        SelectedMythCommandHandler2.Command = new DodgeCommand();

    }

    public void UseRightAbilitySouth(InputAction.CallbackContext context)
    {
        UseAbility2(context, myth => myth.southAbility);
    }

    public void UseRightAbilityWest(InputAction.CallbackContext context)
    {
        UseAbility2(context, myth => myth.westAbility);
    }

    public void UseLeftAbilityNorth(InputAction.CallbackContext context)
    {
        UseAbility1(context, myth => myth.northAbility);
    }

    public void UseLeftAbilityEast(InputAction.CallbackContext context)
    {
        //if (!context.performed) return;
        //CancelManualMovement();
        SelectedMythCommandHandler1.Command = new DodgeCommand();

    }

    public void UseLeftAbilitySouth(InputAction.CallbackContext context)
    {
        UseAbility1(context, myth => myth.southAbility);
    }

    public void UseLeftAbilityWest(InputAction.CallbackContext context)
    {
        UseAbility1(context, myth => myth.westAbility);
    }
    public void MoveLeft(InputAction.CallbackContext context)
    {
        //if (selectedMythIndex != mythsInPlay[0]) return;
        
        if (SelectedMythCommandHandler1.Command is not ManualMoveCommand)
            SelectedMythCommandHandler1.Command = new ManualMoveCommand();

        if (SelectedMythCommandHandler1.Command is ManualMoveCommand manualMoveCommand)
        {
            manualMoveCommand.input = context.ReadValue<Vector2>();
        }
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        //if (selectedMythIndex != mythsInPlay[1]) return;
        if (SelectedMythCommandHandler2.Command is not ManualMoveCommand)
            SelectedMythCommandHandler2.Command = new ManualMoveCommand();

        if (SelectedMythCommandHandler2.Command is ManualMoveCommand manualMoveCommand2)
        {
            manualMoveCommand2.input = context.ReadValue<Vector2>();
        }
    }
    
    private void UseAbility1(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
    {
        //if (!context.performed) return;
        
        //if (!SelectedMyth) return;

        var ability = abilityAccessor(SelectedMyth1);

        // TODO: I don't think this is the right place for this check
        if (SelectedMyth1.Stamina.Value < ability.stamina) return;
        
        if (!ability.isRanged)
        {
            SelectedMythCommandHandler1.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
            Debug.Log("Close range attack");
            return;
        }

        SelectedMythCommandHandler1.Command = new AbilityCommand(ability);
        
        SelectAbility.Invoke(ability);
    }

    private void UseAbility2(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
    {
        //if (!context.performed) return;

        //if (!SelectedMyth) return;

        var ability = abilityAccessor(SelectedMyth2);

        // TODO: I don't think this is the right place for this check
        if (SelectedMyth2.Stamina.Value < ability.stamina) return;

        if (!ability.isRanged)
        {
            SelectedMythCommandHandler2.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
            Debug.Log("Close range attack");
            return;
        }

        SelectedMythCommandHandler2.Command = new AbilityCommand(ability);

        SelectAbility.Invoke(ability);
    }
}
