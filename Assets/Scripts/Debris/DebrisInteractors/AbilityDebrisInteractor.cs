using System.Collections.Generic;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class AbilityDebrisInteractor : DebrisInteractor
    {
        [SerializeField] public Ability ability;
        [SerializeField] private float effectValue;
        [SerializeField] public CreateDebrisInteractor createDebrisInteractor;
        [SerializeField] private ElementFilter positiveEffectElementFilter;
        [SerializeField] private ElementFilter negativeEffectElementFilter;
        
        private List<Debris> debrisTouched = new();

        private float totalEffect = 1;

        internal override void OnDebrisEnter(Debris debris)
        {
            base.OnDebrisEnter(debris);
            
            if (debris.CurrentElement == null) return;
            
            // TODO: All these contains are going to nuke our performance
            // TODO: This one can't really be optimised, but could be moved after the others
            if (debrisTouched.Contains(debris)) return;

            // TODO: This can be optimised in a couple of ways, the first is to use a better data structure like a Dictionary<Vector3, bool>
            // TODO: The second would be to store the ability that created the debris on the debris, though that's a cyclic dependency
            if (createDebrisInteractor && createDebrisInteractor.PlacedDebris.Contains(debris)) return;

            // TODO: These ones can be optimised by compiling the strengths into a matrix
            var abilityElement = ability.ability.element;
            
            if (negativeEffectElementFilter.PassesFilter(abilityElement, debris.CurrentElement))
            {
                totalEffect -= effectValue;
            }

            if (positiveEffectElementFilter.PassesFilter(abilityElement, debris.CurrentElement))
            {
                totalEffect += effectValue;
            }

            debrisTouched.Add(debris);
            
            // TODO: Also need to cap the damage. Here or in Ability?
            
            // TODO: May need to be more flexible to allow other things to modify the damage too
            ability.DamageMultiplier = totalEffect;
        }
    }
}