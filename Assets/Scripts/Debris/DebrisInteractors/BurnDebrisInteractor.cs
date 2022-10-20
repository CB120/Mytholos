using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class BurnDebrisInteractor : ApplyEffectDebrisInteractor
    {
        [SerializeField] private float damagePerSecond;

        protected override void ApplyEffect()
        {
            base.ApplyEffect();
            
            effects.Burn(damagePerSecond);
        }

        protected override void RemoveEffect()
        {
            base.RemoveEffect();
            
            effects.EndBurn();
        }
    }
}