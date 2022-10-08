using System.Collections;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class ChargeAbilityState : State
    {
        [Header("Charge Ability Behaviour")]
        [Tooltip("The percentage of the ability's stamina cost to deduct if cancelled while charging (0.5 is 50%).")]
        [SerializeField] private float staminaPenalty;
        public UnityEvent abilityCharged = new();
        
        private Coroutine chargeAbilityCoroutine;
        private AbilityCommand abilityCommand;

        private bool abilityWasCharged;

        protected override void OnEnable()
        {
            base.OnEnable();

            abilityWasCharged = false;
            
            abilityCommand = mythCommandHandler.Command as AbilityCommand;
            
            if (chargeAbilityCoroutine != null)
                StopCoroutine(chargeAbilityCoroutine);

            chargeAbilityCoroutine = StartCoroutine(ChargeAbility());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (chargeAbilityCoroutine != null)
                StopCoroutine(chargeAbilityCoroutine);

            chargeAbilityCoroutine = null;

            if (!abilityWasCharged && abilityCommand != null)
                // TODO: Unsafe. Does not prevent negative values.
                myth.Stamina.Value -= abilityCommand.abilityData.staminaCost * staminaPenalty;
        }

        private IEnumerator ChargeAbility()
        {
            yield return new WaitForSeconds(abilityCommand.abilityData.chargeTime);

            abilityWasCharged = true;

            abilityCharged.Invoke();
        }
    }
}