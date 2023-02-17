using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class SizeBuff : LastingEffect
    {
        [SerializeField] private Myth myth;
        
        private float defaultSizeStat = 0;

        private void Awake()
        {
            defaultSizeStat = myth.SizeStat;
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            myth.SizeStat = Mathf.Clamp(myth.SizeStat * 2, defaultSizeStat / 2, defaultSizeStat * 2);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            myth.SizeStat = defaultSizeStat;
        }
    }
}