namespace StateMachines.Commands
{
    public class StunCommand : Command
    {
        public float stunTime;

        public StunCommand(float stunTime)
        {
            this.stunTime = stunTime;
        }
    }
}
