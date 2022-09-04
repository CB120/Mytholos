using System.Collections.Generic;
using Commands;
using UnityEngine;
using UnityEngine.Events;

namespace Myths
{
    public class Myth : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour initialState;
        [SerializeField] private ManualMovementStyle manualMovementStyle;
        
        private MonoBehaviour currentState;

        //Events
        public UnityEvent<float> HealthChanged = new();
    
        //Properties
        public SO_Myth myth;
        public float stamina;
        public float walkSpeed;
        public float sprintSpeed;
        public float health;
        public float Health
        {
            get => health;
            set
            {
                //if (value == health) return;

                print("Health changed!");
                health = value;
                HealthChanged.Invoke(health / 100.0f); // We're assuming 100.0f is the maximum health a myth can have
            }
        }
        public int partyIndex;


        //Variables
        List<Command> commandQueue = new List<Command>();

        public bool isInteracting = false; // Not sure if im going to go this route, but when we're using an ability we'll set
                                           // this to true for the duration & make abilities only usable if this is false

        public bool isInvulnerable = false;
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

        #if UNITY_EDITOR
        private void OnValidate()
        {
            Health = health;
        }
        #endif

        private void Start()
        {
            currentState = initialState;

            currentState.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            //Debug.Log($"{gameObject.name}, Has {health} Health Remaining");
            if (health <= 0)
            {
                //Debug.Log($"{gameObject.name}, Has Been Destroyed");
                TemporaryUpdateTeam(); //Remove this after 5/09/22
                this.gameObject.SetActive(false);
                
            }
        }

        /*Remove everything after this after 5/09/22*/
        public WinState ws;
        public void TemporaryUpdateTeam()
        {
            ws.DecreaseScore(partyIndex);
        }
    }
}