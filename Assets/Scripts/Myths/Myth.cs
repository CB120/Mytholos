using System.Collections.Generic;
using Commands;
using UnityEngine;

namespace Myths
{
    public class Myth : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour initialState;
        [SerializeField] private ManualMovementStyle manualMovementStyle;
        
        private MonoBehaviour currentState;
    
        //Properties
        public SO_Myth myth;
        public float stamina;
        public float speed;
        public float acceleration;
        public float health;
        public int partyIndex;


        //Variables
        List<Command> commandQueue = new List<Command>();

        public ManualMovementStyle ManualMovementStyle => manualMovementStyle;


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

        private void Start()
        {
            currentState = initialState;

            currentState.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log($"{gameObject.name}, Has {health} Health Remaining");
            if (health <= 0)
            {
                Debug.Log($"{gameObject.name}, Has Been Destroyed");
                this.gameObject.SetActive(false);
            }
        }
    }
}