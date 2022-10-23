using UnityEngine;

namespace StateMachines.Commands
{
    public class MoveCommand : Command
    {
        public Vector2 input;

        public MoveCommand(Vector2 input)
        {
            this.input = input;
        }
    }
}