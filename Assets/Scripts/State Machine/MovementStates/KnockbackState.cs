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
        
        protected override void OnEnable()
        {
            base.OnEnable();

            knockbackService = mythCommandHandler.Command as KnockbackService;

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
            sender = knockbackService.abilitySender;
            knockbackStrength = knockbackService.knockbackStrength;
            senderStrength = knockbackService.senderStrength;
           
            if(sender == null && knockbackStrength == 0 && senderStrength == 0)
            {
                Invoke("lateEnable", 0.002f);
            } else
            {
                //Debug.Log("Step 3 Knockback complete");
                ActivateKnockback(); // Might go with a co-routine for this instead!
            }
        }

        private void lateEnable()
        {
                sender = knockbackService.abilitySender;
                knockbackStrength = knockbackService.knockbackStrength;
                senderStrength = knockbackService.senderStrength;
            
            if (sender != null && knockbackStrength != 0)
            {
                //Debug.Log("Step 3 Knockback complete");
                ActivateKnockback(); // Might go with a co-routine for this instead!
                return;
            }
            else
            {
                Debug.Log("Knockback failed at step 3");
                knockbackFailed.Invoke();
            }
        }

        private void ActivateKnockback()
        {
            //Debug.Log("ACTIVATINGGGG");
            Vector3 direction = (myth.transform.position - sender.transform.position).normalized;
            // Add force here
            movementController.SetTargetVelocity(direction * (knockbackStrength - (myth.myth.size - senderStrength)));
            // Make a call to reset the knockback effect and bring velocity to 0, then move to stunned.
            Invoke("ResetKnockback", knockbackDelay);

        }

        private void ResetKnockback()
        {
            movementController.SetTargetVelocity(Vector3.zero);
            knockbackComplete.Invoke(); // Move it to stunned
        }
    }
}
