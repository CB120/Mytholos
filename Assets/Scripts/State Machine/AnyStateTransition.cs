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
        public UnityEvent knockbackCommandReceived = new();
        public UnityEvent stunCommandReceived = new();
        public UnityEvent freezeDebrisReceived = new();
        public UnityEvent moveCommandReceived = new();
        public UnityEvent dodgeCommandReceived = new();
        public UnityEvent swapCommandReceived = new();

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
                knockbackCommandReceived.Invoke();
            }

            if (mythCommandHandler.Command is StunService)
            {
                stunCommandReceived.Invoke();
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
            if(mythCommandHandler.Command is SwapCommand)
            {
                swapCommandReceived.Invoke();
            }
        }
    }
}