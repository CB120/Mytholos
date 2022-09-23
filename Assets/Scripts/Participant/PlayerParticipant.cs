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
    private int selectedEnemyIndex = 0;
    private bool EnemySwitch = false; 
    private Myth SelectedMyth => ParticipantData.partyData[partyIndex].myths.ElementAtOrDefault(selectedMythIndex);
    // TODO: Should be cached for performance
    private MythCommandHandler SelectedMythCommandHandler => SelectedMyth.GetComponent<MythCommandHandler>();
    

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
        if (SelectedMythCommandHandler.Command is ManualMoveCommand manualMoveCommand)
            manualMoveCommand.input = Vector2.zero;
    }

    private void CancelCommandMovement()
    {
        if (SelectedMythCommandHandler.Command is ManualMoveCommand manualMoveCommand)
            SelectedMyth.GetComponentInChildren<NavMeshAgent>().ResetPath();
    }

    public void UseAbilityNorth(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.northAbility);
    }

    public void UseAbilityEast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;


        //if (SelectedMyth. < SelectedMyth.eastAbility.stamina) return;
        //SelectedMyth.Stamina -= 30;

        SelectedMythCommandHandler.Command = new DodgeCommand();
        CancelManualMovement();
        //SelectAbility.Invoke(3);
    }

    public void UseAbilitySouth(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.southAbility);
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        UseAbility(context, myth => myth.westAbility);
    }

    public void MoveStrategyUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMythCommandHandler.Command = new MoveCommand(MoveCommand.MoveCommandType.Idle);
    }

    public void MoveStrategyDown(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;
        
        SelectedMythCommandHandler.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
    }

    public void MoveStrategyLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        // TODO: Decide on a movement strategy.
        // SelectedMyth.Command = new MoveCommand();

        SelectedMythCommandHandler.Command = new MoveCommand(MoveCommand.MoveCommandType.Flee);
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

        if (SelectedMythCommandHandler.Command is not ManualMoveCommand)
            SelectedMythCommandHandler.Command = new ManualMoveCommand();

        if (SelectedMythCommandHandler.Command is ManualMoveCommand manualMoveCommand)
        {
            CancelCommandMovement();
            manualMoveCommand.input = context.ReadValue<Vector2>();
        }
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

    private void UseAbility(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
    {
        if (!context.performed) return;
        
        if (!SelectedMyth) return;

        var ability = abilityAccessor(SelectedMyth);

        // TODO: I don't think this is the right place for this check
        if (SelectedMyth.Stamina.Value < ability.stamina) return;
        
        if (!ability.isRanged)
        {
            SelectedMythCommandHandler.Command = new MoveCommand(MoveCommand.MoveCommandType.Approach);
            Debug.Log("Close range attack");
            return;
        }

        SelectedMythCommandHandler.Command = new AbilityCommand(ability);
        
        SelectAbility.Invoke(ability);
    }
}
