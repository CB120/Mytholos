using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{

    public class KnockbackState : State
    {
        // References
        // References
        [SerializeField] private CollisionDetection movementController;
        private KnockbackCommand knockbackService;
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

        [Header("SFX")]
        [SerializeField] GameObject knockbackSFXprefab; //SFX, added by Ethan
        [SerializeField] float timeToDestroySFX = 0.4f;
        [SerializeField] float timeBeforeRepeat = 0.3f;
        float timeStarted = -100f;

        protected override void OnEnable()
        {
            base.OnEnable();
            knockbackService = mythCommandHandler.LastCommand as KnockbackCommand;
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
            Invoke("lateEnable", 0.001f);
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

            //SFX, added by Ethan
            if (timeStarted + timeBeforeRepeat <= Time.time)
            {
                GameObject sfxGameObject = Instantiate(knockbackSFXprefab, transform.position, Quaternion.identity);
                Destroy(sfxGameObject, timeToDestroySFX);
                timeStarted = Time.time;
            }
        }

        private void ResetKnockback()
        {
            movementController.SetTargetVelocity(Vector3.zero);
            knockbackComplete.Invoke();
            mythCommandHandler.PushCommand(new StunCommand(stunTime));
            if (mythCommandHandler.LastCommand is StunCommand stunService)
            {
                //Debug.Log("Is StunService");
                stunService.stunTime = stunTime;
            }
        }
    }
}