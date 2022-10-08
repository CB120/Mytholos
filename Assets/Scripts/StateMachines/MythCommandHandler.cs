using UnityEngine;
using UnityEngine.Events;

namespace StateMachines
{
    public class MythCommandHandler : MonoBehaviour
    {
        private Command command;

        public Command Command
        {
            get => command;
            set
            {
                if (!WillStoreNewCommands) return;
                command = value;
                commandChanged.Invoke();
            }
        }
        
        public bool WillStoreNewCommands { get; set; }

        [HideInInspector] public UnityEvent commandChanged = new();
    }
}