using Myths;
using UnityEngine;
using UnityEngine.Events;

namespace Commands
{
    // TODO: Ideally all this functionality should be broken out into separate classes.
    public class AnyStateTransition : MonoBehaviour
    {
        [SerializeField] protected Myth myth;
        
        public UnityEvent abilityCommandReceived = new();
        public UnityEvent moveCommandReceived = new();
        public UnityEvent manualMoveCommandReceived = new();
        public UnityEvent dodgeCommandReceived = new();

        private void OnEnable()
        {
            myth.commandChanged.AddListener(OnCommandChanged);
        }

        private void OnDisable()
        {
            myth.commandChanged.RemoveListener(OnCommandChanged);
        }

        private void OnCommandChanged()
        {
            if (myth.Command == null) return;

            if (myth.Command is AbilityCommand)
            {
                abilityCommandReceived.Invoke();
            }

            if (myth.Command is MoveCommand)
            {
                moveCommandReceived.Invoke();
            }

            if (myth.Command is ManualMoveCommand)
            {
                manualMoveCommandReceived.Invoke();
            }
            if(myth.Command is DodgeCommand)
            {
                if (myth.isInvulnerable == false)
                {
                    dodgeCommandReceived.Invoke();
                }
            }
        }
    }
}