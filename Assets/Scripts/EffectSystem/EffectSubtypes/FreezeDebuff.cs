using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class FreezeDebuff : LastingEffect
    {
        [System.Serializable]
        struct AlternateEffects
        {
            public SO_Element element;
            public GameObject effectObject;
        }
        
        [SerializeField] private Myth myth;
        [SerializeField] private AlternateEffects alternateIce;
        
        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            myth.Stun(duration);
            
            alternateIce.effectObject.SetActive(true);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            alternateIce.effectObject.SetActive(false);
            
            Instantiate(alternateIce.element.debuffParticle, myth.transform);
        }
    }
}