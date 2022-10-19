using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class BurnDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private Myth myth;
        [SerializeField] private Effects effects;
        [SerializeField] private float damagePerSecond;
        [SerializeField] private ElementFilter elementFilter;

        // TODO: Add a check for if the myth is of the same element as the debris
        internal override void OnDebrisEnter(Debris debris)
        {
            if (elementFilter.PassesFilter(debris.CurrentElement))
                effects.Burn(damagePerSecond);
        }

        internal override void OnDebrisExit(Debris debris)
        {
            if (elementFilter.PassesFilter(debris.CurrentElement))
                effects.EndBurn();
        }
    }
}