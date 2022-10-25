using Myths;

namespace StateMachines.Commands
{ 
    public class SwapCommand : Command
    {
        public Myth mythToSwapIn;
        public PlayerParticipant sendingPlayer;

        public SwapCommand(Myth mythToSwapIn, PlayerParticipant sendingPlayer)
        {
            this.mythToSwapIn = mythToSwapIn;
            this.sendingPlayer = sendingPlayer;
        }
    }
}
