using System.Collections;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace StateMachines.Behaviours
{
    public class PerformAbilityBehaviour : Behaviour
    {
        [Header("Perform Ability Behaviour")]
        public UnityEvent performAbilityComplete = new();

        private Coroutine performAbilityCoroutine;
        private AbilityCommand abilityCommand;

        protected override void OnEnable()
        {
            base.OnEnable();
            
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

            Ability ability = abilityObject.GetComponent<Ability>();
            ability.owningMyth = myth;
            ability.ability = abilityData;
            if (performAbilityCoroutine != null)
                StopCoroutine(performAbilityCoroutine);

            performAbilityCoroutine = StartCoroutine(PerformAbility());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (performAbilityCoroutine != null)
                StopCoroutine(performAbilityCoroutine);

            performAbilityCoroutine = null;
        }

        private IEnumerator PerformAbility()
        {
            yield return new WaitForSeconds(abilityCommand.abilityData.performTime);

            mythCommandHandler.Command = null;
            
            performAbilityComplete.Invoke();
        }
    }
}