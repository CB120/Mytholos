using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;
using FMODUnity;

namespace Commands.Behaviours
{   
    public class StunnedState : Behaviour
    {
        // References & Events
        public UnityEvent stunComplete = new();
        public UnityEvent stunFailed = new();

        private StunService stunService;
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

            stunService = mythCommandHandler.Command as StunService;
            if (stunService.stunTime == 0)
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
            CancelInvoke();
        }

        private void startStun()
        {
            if (anim)
            {
                anim.speed = 1.0f;
                anim.SetBool("Stunned", true);
            }
            stunTime = stunService.stunTime;
            movementController.SetTargetVelocity(Vector3.zero);
            Invoke("killStun", stunTime);
        }

        private void killStun()
        {
            if (anim)
            {
                anim.speed = 1.0f;
                anim.SetBool("Stunned", false);
            }
            //Debug.Log("Killed stun");
            mythCommandHandler.Command = null;
            stunComplete.Invoke();
        }
    }
}
