using UnityEngine;
using UnityEngine.Events;

namespace Commands
{
    public class MythCommandHandler : MonoBehaviour
    {
        private Command command;

        public Command Command
        {
            get => command;
            set
            {
                command = value;
                commandChanged.Invoke();
            }
        }

        public UnityEvent commandChanged = new();
    }
}