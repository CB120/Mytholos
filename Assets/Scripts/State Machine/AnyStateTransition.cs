using Myths;
using UnityEngine;
using UnityEngine.Events;

namespace Commands
{
    // TODO: Ideally all this functionality should be broken out into separate classes.
    public class AnyStateTransition : MonoBehaviour
    {
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;

        public UnityEvent abilityCommandReceived = new();
        public UnityEvent incomingAttackReceived = new();
        public UnityEvent freezeDebrisReceived = new();
        public UnityEvent moveCommandReceived = new();
        public UnityEvent dodgeCommandReceived = new();

        private void OnEnable()
        {
            mythCommandHandler.commandChanged.AddListener(OnCommandChanged);
        }

        private void OnDisable()
        {
            mythCommandHandler.commandChanged.RemoveListener(OnCommandChanged);
        }

        private void OnCommandChanged()
        {
            if (mythCommandHandler.Command == null) return;

            if (mythCommandHandler.Command is AbilityCommand)
            {
                abilityCommandReceived.Invoke();
            }

            if(mythCommandHandler.Command is KnockbackService)
            {
                incomingAttackReceived.Invoke();
            }

            if (mythCommandHandler.Command is MoveCommand)
            {
                moveCommandReceived.Invoke();
            }
            if(mythCommandHandler.Command is DodgeCommand)
            {
                if (myth.isInvulnerable == false)
                {
                    dodgeCommandReceived.Invoke();
                }
            }
        }
    }
}