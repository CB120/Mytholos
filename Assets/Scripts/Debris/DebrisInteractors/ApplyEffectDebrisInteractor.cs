using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class ApplyEffectDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private Myth myth;
        [SerializeField] private bool overrideIfMythElementMatchesDebris;
        [SerializeField] protected Effects effects;
        [SerializeField] private ElementFilter elementFilter;

        internal override void OnDebrisEnter(Debris debris)
        {
            if (overrideIfMythElementMatchesDebris && myth.element == debris.CurrentElement) return;
            
            if (elementFilter.PassesFilter(debris.CurrentElement))
                ApplyEffect();
        }

        internal override void OnDebrisExit(Debris debris)
        {
            if (overrideIfMythElementMatchesDebris && myth.element == debris.CurrentElement) return;
            
            if (elementFilter.PassesFilter(debris.CurrentElement))
                RemoveEffect();
        }

        protected virtual void ApplyEffect() { }

        protected virtual void RemoveEffect() { }
    }
}