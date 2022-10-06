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
        private GameObject sender;

        // Events
        public UnityEvent knockbackRecieved = new();
        public UnityEvent knockbackComplete = new();

        // Properties
        private float knockbackStrength;
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

            if(sender != null && knockbackStrength != 0)
            {
                Invoke("ActivateKnockback", knockbackDelay); // Might go with a co-routine for this instead!
            }
        }

        private void ActivateKnockback()
        {
            Vector3 direction = (myth.transform.position - sender.transform.position).normalized;
            // Add force here
            // Make a call to reset the knockback effect and bring velocity to 0, then move to stunned.

        }
    }
}
