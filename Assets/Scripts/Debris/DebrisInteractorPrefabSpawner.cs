using System.Collections.Generic;
using System.Linq;
using Debris.DebrisInteractors;
using UnityEngine;

namespace Debris
{
    // Instantiates the appropriate default debris interactor prefab for the ability's element. Allows elements of abilities to be easily changed.
    public class DebrisInteractorPrefabSpawner : MonoBehaviour
    {
        [SerializeField] private Ability ability;

        private List<DebrisInteractorManager> debrisInteractorManagers = new();
        
        private void Awake()
        {
            var debrisInteractorObject = Instantiate(ability.ability.element.abilityDebrisInteractorsPrefab, transform);

            // TODO: This needs work
            foreach (var abilityDebrisInteractor in debrisInteractorObject.GetComponents<AbilityDebrisInteractor>())
            {
                abilityDebrisInteractor.ability = ability;
            }
            
            foreach (var createDebrisInteractor in debrisInteractorObject.GetComponents<CreateDebrisInteractor>())
            {
                createDebrisInteractor.ability = ability;
            }

            debrisInteractorManagers = debrisInteractorObject.GetComponents<DebrisInteractorManager>().ToList();
        }

        // TODO: Copied from DebrisINteractorManager. Clean up
        private void OnTriggerEnter(Collider other)
        {
            foreach (var debrisInteractorManager in debrisInteractorManagers)
            {
                debrisInteractorManager.OnDebrisTrigger(other,
                    (debrisInteractor, debris) => debrisInteractor.OnDebrisEnter(debris));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (var debrisInteractorManager in debrisInteractorManagers)
            {
                debrisInteractorManager.OnDebrisTrigger(other,
                    (debrisInteractor, debris) => debrisInteractor.OnDebrisExit(debris));
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach (var debrisInteractorManager in debrisInteractorManagers)
            {
                debrisInteractorManager.OnDebrisTrigger(other,
                    (debrisInteractor, debris) => debrisInteractor.OnDebrisStay(debris));
            }
        }
    }
}