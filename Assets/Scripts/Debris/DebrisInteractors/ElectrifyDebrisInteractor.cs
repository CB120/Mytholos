using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;

namespace Debris.DebrisInteractors
{
    public class ElectrifyDebrisInteractor : DebrisInteractor
    {
        [SerializeField] private DebrisControllerService debrisControllerService;
        [SerializeField] private ElementFilter elementFilter;

        private DebrisController debrisController;
        private HashSet<Debris> debrisTouched = new();
        private HashSet<Debris> debrisElectrified = new();
        
        // TODO: Won't correctly handle multiple debris patches

        private void Start()
        {
            debrisController = debrisControllerService.DebrisController;

            if (debrisController != null) return;
            
            enabled = false;
            throw new Exception($"Could not find {nameof(DebrisController)}.");
        }

        internal override void OnDebrisEnter(Debris debris)
        {
            base.OnDebrisEnter(debris);

            if (!elementFilter.PassesFilter(debris.CurrentElement)) return;

            debrisTouched.Add(debris);

            if (debrisElectrified.Contains(debris)) return;
            
            debrisElectrified = Enumerable.ToHashSet(debrisController.FloodGetTiles(debris,
          debris => elementFilter.PassesFilter(debris.CurrentElement)));
                
            debrisElectrified.ForEach(debris => debris.IsElectrified = true);
        }

        internal override void OnDebrisExit(Debris debris)
        {
            base.OnDebrisExit(debris);
            
            if (!elementFilter.PassesFilter(debris.CurrentElement)) return;

            debrisTouched.Remove(debris);
            
            if (debrisTouched.Count == 0)
                debrisElectrified.ForEach(debris => debris.IsElectrified = false);
        }

        private void OnDestroy()
        {
            foreach (var debris in debrisTouched.Reverse())
            {
                OnDebrisExit(debris);
            }
        }
    }
}