using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{

    public class KnockbackState : Behaviour
    {
        // References
        [SerializeField] private CollisionDetection movementController;
        private KnockbackService knockbackService;
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
            knockbackService = mythCommandHandler.Command as KnockbackService;
            CancelInvoke("ResetKnockback");
            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                return;
            }
            
            if(knockbackService == null)
            {
                Debug.LogWarning("There was a problem with getting the knockback service, please check the Knockback State script.");
                return;
            }
                Invoke("lateEnable", 0.002f);
        }

        private void lateEnable()
        {
                sender = knockbackService.abilitySender;
                knockbackStrength = knockbackService.knockbackStrength;
                senderStrength = knockbackService.senderStrength;
                stunTime = knockbackService.stunTime;
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
            mythCommandHandler.Command = new StunService(stunTime);
            if (mythCommandHandler.Command is StunService stunService)
            {
                //Debug.Log("Is StunService");
                stunService.stunTime = stunTime;
            }

        }
    }
}
