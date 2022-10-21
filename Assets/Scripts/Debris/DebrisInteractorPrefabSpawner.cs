using Debris.DebrisInteractors;
using UnityEngine;
using Utilities;

namespace Debris
{
    // Instantiates the appropriate default debris interactor prefab for the ability's element. Allows elements of abilities to be easily changed.
    // TODO: Name should include Ability
    public class DebrisInteractorPrefabSpawner : MonoBehaviour
    {
        [SerializeField] private Ability ability;
        [SerializeField] private ColliderEvents colliderEvents;

        // Must occur in Start to give ability element time to initialise
        private void Start()
        {
            var debrisInteractorObject = Instantiate(ability.ability.element.abilityDebrisInteractorsPrefab, transform);

            // TODO: This needs work
            foreach (var abilityDebrisInteractor in debrisInteractorObject.GetComponentsInChildren<AbilityDebrisInteractor>())
            {
                abilityDebrisInteractor.ability = ability;
            }
            
            foreach (var createDebrisInteractor in debrisInteractorObject.GetComponentsInChildren<CreateDebrisInteractor>())
            {
                createDebrisInteractor.ability = ability;
            }

            foreach (var debrisInteractorManager in debrisInteractorObject.GetComponentsInChildren<DebrisInteractorManager>())
            {
                debrisInteractorManager.Initialise(colliderEvents);
            }
        }
    }
}