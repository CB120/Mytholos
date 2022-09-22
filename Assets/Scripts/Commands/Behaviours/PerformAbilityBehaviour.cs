using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class PerformAbilityBehaviour : Behaviour
    {
        public UnityEvent performAbilityComplete = new();

        private void Update()
        {
            //Debug.Log($"{myth.name} performed ability.");

            var abilityData = ((AbilityCommand) myth.Command).abilityData;
            
            GameObject ability = abilityData.abilityPrefab;
            
            if (ability)
            {
                Vector3 pos = gameObject.transform.position + abilityData.relativeSpawnPosition;

                GameObject abilityPrefab = abilityData.spawnInWorldSpace ?
                    Instantiate(
                        ability,
                        pos,
                        new Quaternion(0f, 0f, 0f, 0f)
                    )
                    : 
                    Instantiate(
                        ability,
                        pos,
                        new Quaternion(0f, 0f, 0f, 0f),
                        gameObject.transform
                    );
                abilityPrefab.GetComponent<Ability>().owningMyth = myth;
            }
            else
            {
                Debug.LogWarning($"Action was not performed. {abilityData} does not have an assigned {nameof(abilityData.abilityPrefab)}.");
            }

            myth.Command = null;

            performAbilityComplete.Invoke();
        }
    }
}