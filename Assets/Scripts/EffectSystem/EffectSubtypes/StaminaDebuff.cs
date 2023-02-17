using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class StaminaDebuff : LastingEffect
    {
        [SerializeField] private MythStat stamina;
        
        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            float value = stamina.RegenSpeed;
            if (isDebuff) value /= 2;
            else value *= 1.25f;
            stamina.RegenSpeed = Mathf.Clamp(value, 0.25f, 15);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            stamina.RegenSpeed = stamina.defaultRegenSpeed;
        }
    }
}