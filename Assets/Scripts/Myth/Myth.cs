using System;
using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;

public enum E_Myth
{
    Beetle,
    Fox,
    Golem,
    Horse,
    Lizard,
    Mask,
    Snake,
    Stag
}

public class Myth : MonoBehaviour
{
    private enum State { Idle, PerformingAbility, Moving }

    private State currentState;
    
    //Properties
    public E_Myth myth;


    //Variables
    List<Command> commandQueue = new List<Command>();


    //References
    public SO_Ability northAbility;
    public SO_Ability westAbility;
    public SO_Ability southAbility;
    public SO_Ability eastAbility;

    public Command Command { get; set; }

    // And here begins the monolithic state machine

    private void Update()
    {
        StateMachine();
    }

    private void StateMachine()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleBehaviour();
                break;
            case State.PerformingAbility:
                PerformingAbilityBehaviour();
                break;
            case State.Moving:
                MovingBehaviour();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void IdleBehaviour()
    {
        if (Command == null) return;

        Debug.Log("command not null");

        if (Command is AbilityCommand)
        {
            currentState = State.PerformingAbility;
        }

        if (Command is MoveCommand)
        {
            currentState = State.Moving;
        }
    }

    private void PerformingAbilityBehaviour()
    {
        Debug.Log($"{name} performed ability.");

        Command = null;

        currentState = State.Idle;
    }

    private void MovingBehaviour()
    {
        Debug.Log($"{name} moved. {((MoveCommand) Command).CurrentMoveCommandType}");

        Command = null;

        currentState = State.Idle;
    }
}
