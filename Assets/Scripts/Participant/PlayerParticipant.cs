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
    private int selectedEnemyIndex = 0;
    private bool EnemySwitch = false; 
    private Myth SelectedMyth => ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(selectedMythIndex);

    private GameObject PartyParent;

    private void Start()
    {
        foreach(int myth in mythsInPlay)
        {
            selectedMythIndex = mythsInPlay[myth];
            for (int i = 0; i < ParticipantData.partyData.Length; i++)
            {
                if (ParticipantData.partyData[i].participant != this)
                {
                    SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                }
            }

        }
    }

    public void SelectLeft(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
        {
            selectedMythIndex = mythsInPlay[0];
            SelectMyth.Invoke(0);
            for (int i = 0; i < ParticipantData.partyData.Length; i++)
            {
                if (ParticipantData.partyData[i].participant != this)
                {
                    SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                    return;
                }
            }
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
            for (int i = 0; i < ParticipantData.partyData.Length; i++)
            {
                if (ParticipantData.partyData[i].participant != this)
                {
                    SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                    return;
                }
            }
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

    public void EnemyTargetRight(InputAction.CallbackContext context)
    {
        TargetEnemy(context);
    }

    public void EnemyTargetLeft(InputAction.CallbackContext context)
    {
        TargetEnemy(context);
    }

    public void TargetEnemy(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!SelectedMyth) return;

        EnemySwitch = !EnemySwitch;
        
        if (EnemySwitch)
        {
            selectedEnemyIndex = 0;
        } else if (!EnemySwitch)
        {
            selectedEnemyIndex = 1;
        }

        for (int i = 0; i < ParticipantData.partyData.Length; i++)
        {
            if (ParticipantData.partyData[i].participant != this)
            {
                SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                return;
            }
        }
    }
}
