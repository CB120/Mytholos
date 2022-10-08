using UnityEngine;

namespace StateMachines
{
    public class MythStateMachine : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour initialState;

        [Header("The current state of the state machine used for debugging")]
        [SerializeField] private MonoBehaviour currentState;

        private void Awake()
        {
            currentState = initialState;

            currentState.enabled = true;
        }

        public void ChangeState(MonoBehaviour state)
        {
            currentState.enabled = false;
            
            currentState = state;

            currentState.enabled = true;
        }
    }
}