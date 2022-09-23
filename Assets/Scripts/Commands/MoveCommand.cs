namespace Commands
{
    public class MoveCommand : Command
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

        public MoveCommand(MoveCommandType moveCommandType)
        {
            CurrentMoveCommandType = moveCommandType;
        }
    }
}