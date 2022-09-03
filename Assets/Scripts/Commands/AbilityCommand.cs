namespace Commands
{
    public class AbilityCommand : Command
    {
        public SO_Ability ability;

        public AbilityCommand(SO_Ability ability)
        {
            this.ability = ability;
        }
    }
}