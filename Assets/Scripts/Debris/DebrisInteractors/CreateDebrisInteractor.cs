using System.Collections.Generic;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class CreateDebrisInteractor : DebrisInteractor
    {
        [SerializeField] public Ability ability;

        public List<Debris> PlacedDebris { get; } = new();

        internal override void OnDebrisEnter(Debris debris)
        {
            base.OnDebrisEnter(debris);
            
            if (debris.PlaceDebris(ability.ability.element))
                PlacedDebris.Add(debris);
        }
    }
}