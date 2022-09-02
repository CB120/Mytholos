using Commands;
using UnityEngine.Events;

namespace Myths.Behaviours
{
    public class IdleBehaviour : Behaviour
    {
        public UnityEvent abilityCommandReceived = new();
        public UnityEvent moveCommandReceived = new();
        
        private void Update()
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
        }
    }
}