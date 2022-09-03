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