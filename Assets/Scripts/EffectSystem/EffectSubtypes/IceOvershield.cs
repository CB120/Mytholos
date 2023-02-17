using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class IceOvershield : LastingEffect
    {
        [SerializeField] private Myth myth;

        public override void ApplyEffect()
        {
            base.ApplyEffect();

            myth.toBeDamaged.AddListener(OnMythToBeDamaged);
        }

        private void OnMythToBeDamaged()
        {
            RemoveEffect();
            
            // TODO: Negate the damage somehow
        }
    }
}