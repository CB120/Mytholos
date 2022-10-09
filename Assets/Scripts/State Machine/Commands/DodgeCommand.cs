using UnityEngine;
namespace Commands
{
    public class DodgeCommand : Command
    {
        public Vector2 input;

        public DodgeCommand(Vector2 newInput)
        {
            input = newInput;
        }
    }
}
