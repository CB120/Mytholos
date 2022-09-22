using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class PerformAbilityBehaviour : Behaviour
    {
        public UnityEvent performAbilityComplete = new();

        private Coroutine performAbilityCoroutine;
        private AbilityCommand abilityCommand;

        private void OnEnable()
        {
            abilityCommand = mythCommandHandler.Command as AbilityCommand;
            
            var abilityData = abilityCommand.abilityData;
            
            GameObject abilityPrefab = abilityData.abilityPrefab;

            if (!abilityPrefab)
            {
                Debug.LogWarning(
                    $"Action was not performed. {abilityData} does not have an assigned {nameof(abilityData.abilityPrefab)}.");

                return;
            }
            
            Vector3 pos = gameObject.transform.position + abilityData.relativeSpawnPosition;

            GameObject abilityObject = abilityData.spawnInWorldSpace
                ? Instantiate(
                    abilityPrefab,
                    pos,
                    new Quaternion(0f, 0f, 0f, 0f)
                )
                : Instantiate(
                    abilityPrefab,
                    pos,
                    new Quaternion(0f, 0f, 0f, 0f),
                    gameObject.transform
                );
            
            abilityObject.GetComponent<Ability>().owningMyth = myth;

            mythCommandHandler.WillStoreNewCommands = false;

            if (performAbilityCoroutine != null)
                StopCoroutine(performAbilityCoroutine);

            performAbilityCoroutine = StartCoroutine(PerformAbility());
        }

        private void OnDisable()
        {
            mythCommandHandler.WillStoreNewCommands = true;
            
            if (performAbilityCoroutine != null)
                StopCoroutine(performAbilityCoroutine);

            performAbilityCoroutine = null;
        }

        private IEnumerator PerformAbility()
        {
            yield return new WaitForSeconds(abilityCommand.abilityData.performTime);

            mythCommandHandler.Command = null;
            
            mythCommandHandler.WillStoreNewCommands = true;

            performAbilityComplete.Invoke();
        }
    }
}