using UnityEngine;

namespace StateMachines.AnyStateTransitions
{
    public abstract class CommandAnyStateTransition<T> : AnyStateTransition where T : Command
    {
        [SerializeField] protected MythCommandHandler mythCommandHandler;

        private void OnEnable()
        {
            mythCommandHandler.lastCommandChanged.AddListener(OnCommandChanged);
        }

        private void OnDisable()
        {
            mythCommandHandler.lastCommandChanged.RemoveListener(OnCommandChanged);
        }

        protected virtual void OnCommandChanged()
        {
            if (mythCommandHandler.LastCommand is T)
            {
                transitionEvent.Invoke();
            }
        }
    }
}