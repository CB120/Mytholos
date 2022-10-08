using Myths;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines
{
    public abstract class AnyStateTransition<T> : MonoBehaviour where T : Command
    {
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;
        
        [SerializeField] protected UnityEvent transitionEvent = new();

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