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
        public float speed;
        public float acceleration;
        public float health;
        public float Health
        {
            get => health;
            set
            {
                health = value;
                HealthChanged.Invoke(health / 100.0f); // We're assuming 100.0f is the maximum health a myth can have
            }
        }
        public int partyIndex;
        public int PartyIndex
        {
            get => partyIndex;
            set
            {
                partyIndex = value;
                ring.color = value == 0 ? new Color(1.0f, 0.3f, 0.0f, 1.0f)
                    : new Color(0.0f, 0.5f, 1.0f, 1.0f);
            }
        }


        //Variables
        List<Command> commandQueue = new List<Command>();

        public ManualMovementStyle ManualMovementStyle => manualMovementStyle;


        //References
        public SO_Ability northAbility;
        public SO_Ability westAbility;
        public SO_Ability southAbility;
        public SO_Ability eastAbility;

        public SpriteRenderer ring;

        public Command Command { get; set; }

        public void ChangeState(MonoBehaviour state)
        {
            currentState.enabled = false;
            
            currentState = state;

            currentState.enabled = true;
        }

        //#if UNITY_EDITOR
        //private void OnValidate()
        //{
        //    Health = health;
        //}
        //#endif

        private void Start()
        {
            currentState = initialState;

            currentState.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            //Debug.Log($"{gameObject.name}, Has {health} Health Remaining");
            if (Health <= 0)
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