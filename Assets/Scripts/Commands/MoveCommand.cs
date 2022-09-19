namespace Commands
{
    public class MoveCommand : Command
    {
        public enum MoveCommandType
        {
            Stay,
            Approach,
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