using StateMachines.Commands;

namespace StateMachines.AnyStateTransitions
{
    public class DodgeCommandReceived : AnyStateTransition<DodgeCommand>
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