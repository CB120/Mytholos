using System.Collections;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class PerformAbilityState : State
    {
        [Header("Perform Ability Behaviour")]
        public UnityEvent performAbilityComplete = new();

        private Coroutine performAbilityCoroutine;
        private AbilityCommand abilityCommand;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            abilityCommand = mythCommandHandler.LastCommand as AbilityCommand;
            
            // TODO: Sometimes fails when spamming abilities, need to look into this
            if (abilityCommand == null)
            {
                performAbilityComplete.Invoke();
                return;
            }

            var abilityData = abilityCommand.abilityData;
            
            GameObject abilityPrefab = abilityData.abilityPrefab;

            if (!abilityPrefab)
            {
                Debug.LogWarning(
                    $"Action was not performed. {abilityData} does not have an assigned {nameof(abilityData.abilityPrefab)}.");

                return;
            }
            
            Vector3 pos = gameObject.transform.position + abilityData.relativeSpawnPosition;
            Vector3 rot = abilityData.relativeSpawnRotation;
            GameObject abilityObject = abilityData.spawnInWorldSpace
                ? Instantiate(
                    abilityPrefab,
                    pos,
                    new Quaternion(rot.x, rot.y, rot.z, 0f)
                )
                : Instantiate(
                    abilityPrefab,
                    pos,
                    new Quaternion(rot.x, rot.y, rot.z, 0f),
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
            if (anim)
            {
                anim.speed = 1.0f;
                anim.SetBool("Attacking", true);

                if (abilityCommand.abilityData.performTime > 0)
                {
                    //anim.SetTrigger("AttackSpecial");
                    anim.SetTrigger("Charge");
                }
                else
                {
                    anim.SetTrigger("Attack");
                }
            }

            yield return new WaitForSeconds(abilityCommand.abilityData.performTime);

            performAbilityComplete.Invoke();

            if (anim)
                anim.SetBool("Attacking", false);
        }
    }
}