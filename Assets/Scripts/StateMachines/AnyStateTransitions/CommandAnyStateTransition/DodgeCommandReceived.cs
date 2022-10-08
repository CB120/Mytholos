using StateMachines.Commands;

namespace StateMachines.AnyStateTransitions.CommandAnyStateTransition
{
    public class DodgeCommandReceived : CommandAnyStateTransition<DodgeCommand>
    {
        protected override void OnCommandChanged()
        {
            if (mythCommandHandler.Command is DodgeCommand)
            {
                if (myth.isInvulnerable == false)
                {
                    transitionEvent.Invoke();
                }
            }
        }
    }
}