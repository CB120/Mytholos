using UnityEngine;
using UnityEngine.Events;

namespace StateMachines
{
    public class MythCommandHandler : MonoBehaviour
    {
        // The most recent command to be pushed to the command handler
        public Command LastCommand { get; private set; }

        // The command currently being processed
        public Command CurrentCommand
        {
            get => currentCommand;
            private set
            {
                currentCommand = value;
                currentCommandType = currentCommand == null ? "null" : currentCommand.GetType().FullName;
            }
        }

        public bool WillStoreNewCommands { get; set; }

        [HideInInspector] public UnityEvent lastCommandChanged = new();

        [Header("Debug Only")]
        [SerializeField] private string currentCommandType;

        private Command currentCommand;

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

        public void DemoteCurrentCommand()
        {
            CurrentCommand = null;
        }
    }
}