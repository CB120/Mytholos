namespace Myths
{
    [System.Serializable]
    public class MythData //used sort of like a struct, bundling together a Participant's Myth selection and their ability choices for PartyBuilder
    {
        public SO_Myth myth;
        public SO_Ability northAbility;
        public SO_Ability westAbility;
        public SO_Ability southAbility;
        public SO_Ability eastAbility;
        
    }
}