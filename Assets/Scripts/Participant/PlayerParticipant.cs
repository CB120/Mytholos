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

    public void SelectLeft(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
            selectedMythIndex = mythsInPlay[0];

        if (selectedMythIndex == mythsInPlay[0] && context.canceled)
        {
            SelectedMyth.ManualMovementStyle.Move(Vector2.zero);
            selectedMythIndex = -1;
        }
    }
    
    public void SelectRight(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
            selectedMythIndex = mythsInPlay[1];

        if (selectedMythIndex == mythsInPlay[1] && context.canceled)
        {
            SelectedMyth.ManualMovementStyle.Move(Vector2.zero);
            selectedMythIndex = -1;
        }
    }

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new AbilityCommand(SelectedMyth.northAbility);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new AbilityCommand(SelectedMyth.eastAbility);
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new AbilityCommand(SelectedMyth.southAbility);
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new AbilityCommand(SelectedMyth.westAbility);
    }

    public void MoveStrategyUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Stay);
    }

    public void MoveStrategyDown(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
    }

    public void MoveStrategyLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        // TODO: Decide on a movement strategy.
        // SelectedMyth.Command = new MoveCommand();

        Debug.Log($"{nameof(MoveStrategyLeft)} has not been set up. See the {nameof(PlayerParticipant)} script.");
    }

    public void MoveStrategyRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        // TODO: Decide on a movement strategy.
        // SelectedMyth.Command = new MoveCommand();
        
        Debug.Log($"{nameof(MoveStrategyRight)} has not been set up. See the {nameof(PlayerParticipant)} script.");
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!SelectedMyth) return;
        
        SelectedMyth.ManualMovementStyle.Move(context.ReadValue<Vector2>());
    }
    
    // TODO: Temp. Just an anim. Will be replaced by abilities.
    public void North(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMyth.ManualMovementStyle.AttackExample();
    }
}
