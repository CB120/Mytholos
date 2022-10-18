using UnityEngine;

namespace StateMachines.Commands
{
    public class KnockbackCommand : Command
    {
        public GameObject abilitySender;
        public float knockbackStrength;
        public float senderStrength;
        public float stunTime;

        public KnockbackCommand(GameObject abilitySender, float knockbackStrength, float senderStrength, float stunTime)
        {
            this.abilitySender = abilitySender;
            this.knockbackStrength = knockbackStrength;
            this.senderStrength = senderStrength;
            this.stunTime = stunTime;
        }
    }
}
