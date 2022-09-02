namespace Commands
{
    public class MoveCommand : Command
    {
        public enum MoveCommandType
        {
            Stay,
            Approach
        }
        
        public MoveCommandType CurrentMoveCommandType { get; set; }

        public MoveCommand(MoveCommandType moveCommandType)
        {
            CurrentMoveCommandType = moveCommandType;
        }
    }
}