using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class AgilityBuff : LastingEffect
    {
        [SerializeField] private Myth myth;
        
        private float defaultWalkSpeed = 0;

        private void Awake()
        {
            defaultWalkSpeed = myth.walkSpeed;
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            myth.walkSpeed = Mathf.Clamp(myth.walkSpeed * 2, 0.5f, defaultWalkSpeed * 2);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            myth.walkSpeed = defaultWalkSpeed;
        }
    }
}