using UnityEngine;
using UnityEngine.Events;
using Commands;
using System.Collections;

namespace Myths.Behaviours
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
                StartCoroutine(PerformAbility(ability, abilityData.chargeTime));
            }
            else
            {
                Debug.LogWarning($"Action was not performed. {abilityData} does not have an assigned {nameof(abilityData.abilityPrefab)}.");
            }

            myth.Command = null;

            performAbilityComplete.Invoke();
        }
        IEnumerator PerformAbility(GameObject ability, float chargeTime)
        {
            yield return new WaitForSeconds(chargeTime);

            GameObject abilityPrefab = Instantiate(
                    ability,
                    gameObject.transform.position,
                    new Quaternion(0f, 0f, 0f, 0f),
                    gameObject.transform
                );
            abilityPrefab.GetComponent<Ability>().owningMyth = myth;
        }
    }
   
}