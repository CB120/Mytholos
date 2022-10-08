using StateMachines.Commands;

namespace Commands
{
    public class AIMoveCommand : Command
    {
        public enum MoveCommandType
        {
            Idle,
            Approach,
            ApproachAttack,
            Flee,
            Support
        }
        
        public MoveCommandType CurrentMoveCommandType { get; set; }

        public AIMoveCommand(MoveCommandType moveCommandType)
        {
            CurrentMoveCommandType = moveCommandType;
        }
    }
}