using System.Collections;
using FMODUnity;
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

        [Header("Beam SFX")]
        public StudioEventEmitter beamChargeSFX;
        float beamChargeTimer = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();

            abilityWasCharged = false;
            
            abilityCommand = mythCommandHandler.LastCommand as AbilityCommand;
            
            if (chargeAbilityCoroutine != null)
                StopCoroutine(chargeAbilityCoroutine);

            chargeAbilityCoroutine = StartCoroutine(ChargeAbility());

            //SFX
            beamChargeSFX.enabled = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (chargeAbilityCoroutine != null)
                StopCoroutine(chargeAbilityCoroutine);

            chargeAbilityCoroutine = null;

            if (!abilityWasCharged && abilityCommand != null)
                myth.Stamina.Value -= abilityCommand.abilityData.staminaCost * staminaPenalty;

            //SFX
            beamChargeSFX.enabled = false;
            beamChargeTimer = 0f;
        }

        private void Update()
        {
            beamChargeTimer += Time.deltaTime;
            beamChargeSFX.SetParameter("Beam Progress", beamChargeTimer / abilityCommand.abilityData.chargeTime * 100);
        }

        private IEnumerator ChargeAbility()
        {
            yield return new WaitForSeconds(abilityCommand.abilityData.chargeTime);

            abilityWasCharged = true;

            abilityCharged.Invoke();
        }
    }
}