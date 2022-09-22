using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class ChargeAbilityBehaviour : Behaviour
    {
        [Header("Charge Ability Behaviour")]
        public UnityEvent abilityCharged = new();
        
        private Coroutine chargeAbilityCoroutine;
        private AbilityCommand abilityCommand;

        protected override void OnEnable()
        {
            base.OnEnable();
            
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
        }

        private IEnumerator ChargeAbility()
        {
            yield return new WaitForSeconds(abilityCommand.abilityData.chargeTime);

            abilityCharged.Invoke();
        }
    }
}