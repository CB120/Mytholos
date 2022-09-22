using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class ChargeAbilityBehaviour : Behaviour
    {
        public UnityEvent abilityCharged = new();
        
        private Coroutine chargeAbilityCoroutine;
        private AbilityCommand abilityCommand;
        
        private void OnEnable()
        {
            abilityCommand = myth.Command as AbilityCommand;
            
            if (chargeAbilityCoroutine != null)
                StopCoroutine(chargeAbilityCoroutine);

            chargeAbilityCoroutine = StartCoroutine(ChargeAbility());
        }

        private void OnDisable()
        {
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