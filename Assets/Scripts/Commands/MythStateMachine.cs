using UnityEngine;

namespace Commands
{
    public class MythStateMachine : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour initialState;
        
        private MonoBehaviour currentState;

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