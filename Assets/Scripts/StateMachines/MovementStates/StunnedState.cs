using Myths;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.MovementStates
{   
    public class StunnedState : State
    {
        // References & Events
        public UnityEvent stunComplete = new();
        public UnityEvent stunFailed = new();

        private StunCommand stunCommand;
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

            stunCommand = mythCommandHandler.Command as StunCommand;
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
            stunTime = stunCommand.stunTime;
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
