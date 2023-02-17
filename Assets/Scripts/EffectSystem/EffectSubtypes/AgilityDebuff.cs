using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class AgilityDebuff : LastingEffect
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
            
            myth.walkSpeed = Mathf.Clamp(myth.walkSpeed /2, defaultWalkSpeed/2, defaultWalkSpeed * 2);
                Invoke("RemoveAgilityDebuff", duration);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            myth.walkSpeed = defaultWalkSpeed;
        }
    }
}