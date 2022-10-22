using StateMachines.Commands;

namespace StateMachines.AnyStateTransitions.CommandAnyStateTransition
{
    public class DodgeCommandReceived : CommandAnyStateTransition<DodgeCommand>
    {
        protected override void OnCommandChanged()
        {
            if (mythCommandHandler.LastCommand is DodgeCommand)
            {
                if (myth.isInvulnerable == false)
                {
                    Activate();
                }
            }
        }
    }
}