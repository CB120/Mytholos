using UnityEngine;

namespace StateMachines.AnyStateTransitions
{
    public abstract class CommandAnyStateTransition<T> : AnyStateTransition where T : Command
    {
        [SerializeField] protected MythCommandHandler mythCommandHandler;

        private void OnEnable()
        {
            mythCommandHandler.commandChanged.AddListener(OnCommandChanged);
        }

        private void OnDisable()
        {
            mythCommandHandler.commandChanged.RemoveListener(OnCommandChanged);
        }

        protected virtual void OnCommandChanged()
        {
            if (mythCommandHandler.Command is T)
            {
                transitionEvent.Invoke();
            }
        }
    }
}