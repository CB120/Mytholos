using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class Burn : LastingEffect
    {
        [SerializeField] private MythStat health;
        [SerializeField] private float damagePerSecond;
        
        private bool burning;

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            burning = true;
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            burning = false;
        }

        private void Update()
        {
            if (burning)
            {
                health.Value -= damagePerSecond * Time.deltaTime;
            }
        }
    }
}