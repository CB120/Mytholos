using System.Linq;
using Commands;
using Myths;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerParticipant : Participant
{
    //Events
    public UnityEvent<int> SelectMyth = new();
    public UnityEvent<int> SelectAbility = new();

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
        {
            selectedMythIndex = mythsInPlay[0];
            SelectMyth.Invoke(0);
        }

        if (selectedMythIndex == mythsInPlay[0] && context.canceled)
        {
            SelectedMyth.ManualMovementStyle.Move(Vector2.zero);
            selectedMythIndex = -1;
            SelectMyth.Invoke(-1);
        }
    }
    
    public void SelectRight(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
        {
            selectedMythIndex = mythsInPlay[1];
            SelectMyth.Invoke(1);
        }

        if (selectedMythIndex == mythsInPlay[1] && context.canceled)
        {
            SelectedMyth.ManualMovementStyle.Move(Vector2.zero);
            selectedMythIndex = -1;
            SelectMyth.Invoke(-1);
        }
    }

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        if (SelectedMyth.Stamina < SelectedMyth.northAbility.stamina) return;   


        SelectedMyth.Command = new AbilityCommand(SelectedMyth.northAbility);
        SelectAbility.Invoke(0);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        if (SelectedMyth.Stamina < SelectedMyth.eastAbility.stamina) return;


        SelectedMyth.Command = new AbilityCommand(SelectedMyth.eastAbility);
        SelectAbility.Invoke(3);
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        if (SelectedMyth.Stamina < SelectedMyth.southAbility.stamina) return;

        SelectedMyth.Command = new AbilityCommand(SelectedMyth.southAbility);
        SelectAbility.Invoke(2);
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        if (SelectedMyth.Stamina < SelectedMyth.westAbility.stamina) return;
        
        SelectedMyth.Command = new AbilityCommand(SelectedMyth.westAbility);
        SelectAbility.Invoke(1);
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

    public void EnemySelectRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!SelectedMyth) return;
    }

    public void EnemySelectLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!SelectedMyth) return;

        /*for(int i = 0; i < liveParticipantData.partyData.Length; i++)
        {
            Debug.Log(liveParticipantData.partyData[i]);
            for (int y = 0; y < liveParticipantData.partyData[i].mythData.Length; y++)
            Debug.Log(liveParticipantData.partyData[i].mythData[y]);
        }*/

        for (int i = 0; i < liveParticipantData.partyData.Length; i++)
        {
            if (liveParticipantData.partyData[i].participant != this)
            {
                Debug.Log(liveParticipantData.partyData[i].myths.Count);
                int tempNumber = liveParticipantData.partyData[i].myths.Count - 1;
                    SelectedMyth.targetEnemy = liveParticipantData.partyData[i].myths[tempNumber].gameObject;
            }
            else
            {
                Debug.Log("found ourself");
            }
        }

    }
}
