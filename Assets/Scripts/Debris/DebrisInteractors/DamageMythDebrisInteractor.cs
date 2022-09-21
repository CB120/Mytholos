using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class DamageMythDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private Myth myth;
        [SerializeField] private float damagePerSecond;
        [SerializeField] private ElementFilter elementFilter;

        // TODO: Add a check for if the myth is of the same element as the debris
        internal override void OnDebrisStay(Debris debris)
        {
            if (elementFilter.PassesFilter(debris.CurrentElement)) 
                myth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}