using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{   
    public class StunnedState : Behaviour
    {
        // References & Events
        public UnityEvent stunComplete = new();
        public UnityEvent stunFailed = new();

        private StunService stunService;
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animation anim;

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
            //Debug.Log("Is this activating");
            Invoke("startStun", 0.1f);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CancelInvoke();
        }

        private void startStun()
        {
            stunTime = stunService.stunTime;
            movementController.SetTargetVelocity(Vector3.zero);
            Invoke("killStun", stunTime);
        }

        private void killStun()
        {
            //Debug.Log("Killed stun");
            mythCommandHandler.Command = null;
            stunComplete.Invoke();
        }

        
    }
}
