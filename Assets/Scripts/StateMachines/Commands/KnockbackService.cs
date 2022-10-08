using UnityEngine;

namespace StateMachines.Commands
{
    public class KnockbackService : Command
    {
        public GameObject abilitySender;
        public float knockbackStrength;
        public float senderStrength;
        public float stunTime;
    }
}
