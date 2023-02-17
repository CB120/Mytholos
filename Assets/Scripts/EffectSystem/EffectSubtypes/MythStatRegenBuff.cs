using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class MythStatRegenBuff : LastingEffect
    {
        [SerializeField] private MythStat mythStat;
        [SerializeField] private float regenSpeedMultiplier;
        
        public override void ApplyEffect()
        {
            base.ApplyEffect();

            mythStat.RegenSpeed *= regenSpeedMultiplier;
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();

            mythStat.RegenSpeed /= regenSpeedMultiplier;
        }
    }
}