using System.Collections;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class PerformAbilityState : State
    {
        [Header("Perform Ability Behaviour")]
        [SerializeField] private float rotationSpeed;
        public UnityEvent performAbilityComplete = new();

        private Coroutine performAbilityCoroutine;
        private AbilityCommand abilityCommand;

        private Ability ability;

        // TODO: Weird dependency.
        private bool abilityIsBeam;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            abilityCommand = mythCommandHandler.CurrentCommand as AbilityCommand;
            
            if (abilityCommand == null)
            {
                Debug.LogWarning($"{nameof(AbilityCommand)} was null.");
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
            
            Vector3 pos = gameObject.transform.position + transform.TransformVector(abilityData.relativeSpawnPosition);
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

            ability = abilityObject.GetComponent<Ability>();
            abilityIsBeam = ability is BeamAbility;
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

        private void Update()
        {
            if (!abilityIsBeam) return;

            // TODO: Would be more efficient to listen to mythCommandHandler.lastCommandChanged
            if (mythCommandHandler.LastCommand is not MoveCommand moveCommand) return;
            
            // TODO: Duplicate code. See MoveState.Update
            var inputVector = new Vector3(
                moveCommand.input.x,
                0,
                moveCommand.input.y
            );

            if (inputVector == Vector3.zero) return;
            
            myth.gameObject.transform.rotation = Quaternion.Slerp(myth.gameObject.transform.rotation, Quaternion.LookRotation(inputVector), Time.deltaTime * rotationSpeed);
        }
    }
}