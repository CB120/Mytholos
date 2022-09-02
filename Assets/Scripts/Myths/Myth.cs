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
        [SerializeField] private MonoBehaviour initialState;
        private MonoBehaviour currentState;
    
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

        public void ChangeState(MonoBehaviour state)
        {
            currentState.enabled = false;
            
            currentState = state;

            currentState.enabled = true;
        }
        
        // TODO: From Baxter, left in for testing but will be removed once the state machine is ready.
        //Input-called
        public virtual void OnNorthPress() //Xbox -> Y | PlayStation -> Triangle | Switch -> X
        {
            print("North Pressed");
            GameObject obj = Instantiate(northAbility.ability.gameObject, this.gameObject.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.gameObject.transform);
        }

        private void Start()
        {
            currentState = initialState;

            currentState.enabled = true;
        }
    }
}