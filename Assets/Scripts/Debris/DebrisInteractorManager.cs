using System;
using System.Collections.Generic;
using UnityEngine;

namespace Debris
{
    public class DebrisInteractorManager : MonoBehaviour
    {
        [Tooltip("Determines the order in which DebrisInteractors will be called.")]
        [SerializeField] private List<DebrisInteractor> debrisInteractors = new();
        
        private void OnTriggerEnter(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisEnter(debris));
        }

        private void OnTriggerExit(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisExit(debris));
        }

        private void OnTriggerStay(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisStay(debris));
        }

        public void OnDebrisTrigger(Collider other, Action<DebrisInteractor, Debris> action)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;

            foreach (var debrisInteractor in debrisInteractors)
            {
                action(debrisInteractor, debris);
            }
        }
    }
}