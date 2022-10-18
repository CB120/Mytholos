using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{   
    public class StunnedState : State
    {
        // References & Events
        public UnityEvent stunComplete = new();
        public UnityEvent stunFailed = new();

        private StunCommand stunCommand;
        [SerializeField] private CollisionDetection movementController;

        [Header("SFX")]
        [SerializeField] GameObject stunnedSFXprefab; //SFX, added by Ethan
        [SerializeField] float timeToDestroySFX = 4f;

        private float stunTime;
        protected override void OnEnable()
        {
            base.OnEnable();
            CancelInvoke();
            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                stunFailed.Invoke();
                return;
            }

            stunCommand = mythCommandHandler.LastCommand as StunCommand;
            if (stunCommand.stunTime == 0)
            {
                stunFailed.Invoke();
                //Debug.Log("Stun time is 0");
            }
            else
            {
                //Debug.Log("Is this activating");
                Invoke("startStun", 0.1f);

                //SFX, added by Ethan
                GameObject sfxGameObject = Instantiate(stunnedSFXprefab, transform.position, Quaternion.identity);
                Destroy(sfxGameObject, timeToDestroySFX);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (anim)
            {
                anim.speed = 1.0f;
                anim.SetBool("Stunned", false);
            }
            
            CancelInvoke();
        }

        private void startStun()
        {
            if (anim)
            {
                anim.speed = 1.0f;
                anim.SetBool("Stunned", true);
            }
            stunTime = stunCommand.stunTime;
            movementController.SetTargetVelocity(Vector3.zero);
            Invoke("killStun", stunTime);
        }

        private void killStun()
        {
            //Debug.Log("Killed stun");
            stunComplete.Invoke();
        }
    }
}
