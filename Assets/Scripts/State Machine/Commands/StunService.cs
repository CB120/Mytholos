using UnityEngine;
namespace Commands
{
    public class StunService : Command
    {
        public float stunTime;

        public StunService(float stunTime)
        {
            this.stunTime = stunTime;
        }
    }
}
