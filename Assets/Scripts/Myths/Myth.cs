using System;
using System.Collections.Generic;
using Commands;
using UnityEngine;

namespace Myths
{
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
        public enum State { Idle, PerformingAbility, Moving }

        private State currentState;
    
        //Properties
        public E_Myth myth;
        public float stamina;
        public float speed;
        public float acceleration;
        public float health;


        //Variables
        List<Command> commandQueue = new List<Command>();


        //References
        public SO_Ability northAbility;
        public SO_Ability westAbility;
        public SO_Ability southAbility;
        public SO_Ability eastAbility;

        public Command Command { get; set; }

        // And here begins the monolithic state machine

        private void LateUpdate()
        {
            StateMachine();
        }

        private void StateMachine()
        {
        }

        public void ChangeState(State state)
        {
            currentState = state;
            
            
        }
    }
}