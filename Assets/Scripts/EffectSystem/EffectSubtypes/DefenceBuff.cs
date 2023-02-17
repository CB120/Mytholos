using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class DefenceBuff : LastingEffect
    {
        [SerializeField] private Myth myth;
        
        private float defaultDefenceStat;
        
        private void Awake()
        {
            defaultDefenceStat = myth.DefenceStat;
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            myth.DefenceStat = Mathf.Clamp(myth.DefenceStat * 2, defaultDefenceStat/2, defaultDefenceStat*2);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            myth.DefenceStat = defaultDefenceStat;
        }
    }
}