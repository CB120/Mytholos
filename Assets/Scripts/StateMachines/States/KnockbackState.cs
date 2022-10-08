using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{

    public class KnockbackState : State
    {
        // References
        [SerializeField] private CollisionDetection movementController;
        private KnockbackCommand knockbackCommand;
        [SerializeField] private GameObject sender;
        


        // Events
        public UnityEvent knockbackFailed = new();
        public UnityEvent knockbackComplete = new();

        // Properties
        private float knockbackStrength;
        private float senderStrength;
        [SerializeField] private float knockbackDelay = 0.2f;

        // Stun properties to pass through
        private float stunTime;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            knockbackCommand = mythCommandHandler.Command as KnockbackCommand;
            CancelInvoke("ResetKnockback");
            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                return;
            }
            
            if(knockbackCommand == null)
            {
                Debug.LogWarning("There was a problem with getting the knockback service, please check the Knockback State script.");
                return;
            }
                Invoke("lateEnable", 0.002f);
        }

        private void lateEnable()
        {
                sender = knockbackCommand.abilitySender;
                knockbackStrength = knockbackCommand.knockbackStrength;
                senderStrength = knockbackCommand.senderStrength;
                stunTime = knockbackCommand.stunTime;
            if (sender != null && knockbackStrength != 0)
            {
                //Debug.Log("Step 3 Knockback complete");
                ActivateKnockback(); // Might go with a co-routine for this instead!
            }
            else
            {
                Debug.Log("Knockback failed at step 3");
                knockbackFailed.Invoke();
            }
        }

        private void ActivateKnockback()
        {
            Vector3 direction = (myth.transform.position - sender.transform.position).normalized;
            movementController.SetTargetVelocity(direction * (knockbackStrength - (myth.myth.size - senderStrength)));
            Invoke("ResetKnockback", knockbackDelay);
        }

        private void ResetKnockback()
        {
            movementController.SetTargetVelocity(Vector3.zero);
            mythCommandHandler.Command = null;
            knockbackComplete.Invoke();
            mythCommandHandler.Command = new StunCommand(stunTime);
            if (mythCommandHandler.Command is StunCommand stunService)
            {
                //Debug.Log("Is StunService");
                stunService.stunTime = stunTime;
            }

        }
    }
}
