using UnityEngine;

namespace StateMachines.Commands
{
    public class KnockbackCommand : Command
    {
        public GameObject abilitySender;
        public float knockbackStrength;
        public float senderStrength;
        public float stunTime;
    }
}
