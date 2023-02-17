using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class StaminaBuff : Effect
    {
        [SerializeField] private MythStat stamina;
        
        // TODO: Never called?
        public void ApplyEffect(float increase)
        {
            base.ApplyEffect();
            
            stamina.Value += increase;
        }
    }
}