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
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;

            foreach (var debrisInteractor in debrisInteractors)
            {
                debrisInteractor.OnDebrisEnter(debris);
            }
        }
    }
}