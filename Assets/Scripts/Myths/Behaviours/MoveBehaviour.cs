using Commands;
using UnityEngine;
using UnityEngine.Events;

namespace Myths.Behaviours
{
    public class MoveBehaviour : Behaviour
    {
        public UnityEvent moveComplete = new();
        
        private void Update()
        {
            Debug.Log($"{myth.name} moved. {((MoveCommand) myth.Command).CurrentMoveCommandType}");

            myth.Command = null;

            moveComplete.Invoke();
        }
    }
}