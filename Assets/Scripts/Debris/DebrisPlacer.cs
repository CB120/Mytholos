using System.Collections.Generic;
using UnityEngine;

namespace Debris
{
    // TODO: CreateDebrisInteractor
    public class DebrisPlacer : DebrisInteractor
    {
        // TODO: Element should be taken from the ability
        [SerializeField] private SO_Element element;

        public List<Debris> PlacedDebris { get; } = new();

        internal override void OnDebrisEnter(Debris debris)
        {
            base.OnDebrisEnter(debris);
            
            if (debris.PlaceDebris(element))
                PlacedDebris.Add(debris);
        }
    }
}