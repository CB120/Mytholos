using UnityEngine;

namespace StateMachines.Commands
{ 
    public class SwapCommand : Command
    {
        public GameObject SwappingInMyth;
        public int PartyIndex;
        public int TriggerIndex;
        public PlayerParticipant sendingPlayer;
    }
}
