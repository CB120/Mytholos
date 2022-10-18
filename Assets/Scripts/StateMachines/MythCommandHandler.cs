using UnityEngine;
using UnityEngine.Events;

namespace StateMachines
{
    public class MythCommandHandler : MonoBehaviour
    {
        // The most recent command to be pushed to the command handler
        public Command LastCommand { get; private set; }

        // The command currently being processed
        public Command CurrentCommand { get; private set; }

        public bool WillStoreNewCommands { get; set; }

        [HideInInspector] public UnityEvent lastCommandChanged = new();

        public void PushCommand(Command command)
        {
            if (!WillStoreNewCommands) return;
            
            LastCommand = command;

            lastCommandChanged.Invoke();
        }

        public void PromoteLastCommand()
        {
            CurrentCommand = LastCommand;
        }
    }
}