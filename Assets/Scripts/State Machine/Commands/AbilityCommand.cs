namespace Commands
{
    public class AbilityCommand : Command
    {
        public SO_Ability abilityData;

        public AbilityCommand(SO_Ability abilityData)
        {
            this.abilityData = abilityData;
        }
    }
}