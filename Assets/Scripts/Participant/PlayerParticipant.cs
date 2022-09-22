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
    //public static PlayerParticipant singleton;

    //public List<GameObject> Enemies;

    private void Start()
    {

        // Add code here to only do this in game instead of on start
        foreach(int myth in mythsInPlay)
        {
            selectedMythIndex = mythsInPlay[myth];
            for (int i = 0; i < ParticipantData.partyData.Length; i++)
            {
                if (ParticipantData.partyData[i].participant != this)
                {
                    SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                    //Enemies.Add(ParticipantData.partyData[i].myths[0].gameObject);
                }
            }
        }
        selectedMythIndex = -1;
    }

    public void SelectLeft(InputAction.CallbackContext context)
    {
        if (selectedMythIndex == -1 && context.performed)
        {
            selectedMythIndex = mythsInPlay[0];
            SelectMyth.Invoke(0);
        }

        if (selectedMythIndex == mythsInPlay[0] && context.canceled)
        {
            CancelManualMovement();
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
            CancelManualMovement();
            selectedMythIndex = -1;
            SelectMyth.Invoke(-1);
        }
    }

    private void CancelManualMovement()
    {
        if (SelectedMyth.Command is ManualMoveCommand manualMoveCommand)
            manualMoveCommand.input = Vector2.zero;
    }

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        if (SelectedMyth.Stamina < SelectedMyth.northAbility.stamina) return;
        if (!SelectedMyth.northAbility.isRanged)
        {
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
            Debug.Log("Close range attack");
            return;
        }

        SelectedMyth.Command = new AbilityCommand(SelectedMyth.northAbility);
        SelectAbility.Invoke(0);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        
        //if (SelectedMyth.Stamina < SelectedMyth.eastAbility.stamina) return;
        

        SelectedMyth.Command = new DodgeCommand();
        //SelectAbility.Invoke(3);
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        if (SelectedMyth.Stamina < SelectedMyth.southAbility.stamina) return;

        if (!SelectedMyth.southAbility.isRanged)
        {
            SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.ApproachAttack);
            SelectedMyth.Command = new AbilityCommand(SelectedMyth.southAbility);
            Debug.Log("Close range attack");
            return;
        }
        else
        {
            SelectedMyth.Command = new AbilityCommand(SelectedMyth.southAbility);
            SelectAbility.Invoke(2);
        }
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
        
        SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Idle);
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

        SelectedMyth.Command = new MoveCommand(MoveCommand.MoveCommandType.Flee);
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

        if (SelectedMyth.Command is not ManualMoveCommand)
            SelectedMyth.Command = new ManualMoveCommand();
        
        var manualMoveCommand = SelectedMyth.Command as ManualMoveCommand;

        manualMoveCommand.input = context.ReadValue<Vector2>();
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
                if (ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject.activeSelf == false)
                {
                    return;
                }
                else
                {
                    SelectedMyth.targetEnemy = ParticipantData.partyData[i].myths[selectedEnemyIndex].gameObject;
                }
                return;
            }
        }
    }
}
