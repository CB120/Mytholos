using System.Collections.Generic;
using Myths;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class SlidingDebrisInteractor : DebrisInteractor
    {
        // TODO: Duplicate code. See ApplyEffectDebrisInteractor
        [SerializeField] private Myth myth;
        [SerializeField] private bool overrideIfMythElementMatchesDebris;
        [SerializeField] protected CollisionDetection collisionDetection;
        [SerializeField] private ElementFilter elementFilter;

        private HashSet<Debris> debrisInContact = new();

        internal override void OnDebrisEnter(Debris debris)
        {
            if (overrideIfMythElementMatchesDebris && myth.element == debris.CurrentElement) return;

            if (!elementFilter.PassesFilter(debris.CurrentElement)) return;

            debrisInContact.Add(debris);
            
            collisionDetection.IsSliding = true;
        }

        internal override void OnDebrisExit(Debris debris)
        {
            if (overrideIfMythElementMatchesDebris && myth.element == debris.CurrentElement) return;
            
            if (!elementFilter.PassesFilter(debris.CurrentElement)) return;

            debrisInContact.Remove(debris);
            
            if (debrisInContact.Count == 0)
                collisionDetection.IsSliding = false;
        }
    }
}