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
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animation anim;

        public float stunTime = 2f;
        protected override void OnEnable()
        {
            base.OnEnable();

            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                stunFailed.Invoke();
                return;
            }

            movementController.SetTargetVelocity(Vector3.zero);
            Invoke("killStun", stunTime);
        }

        private void killStun()
        {
            mythCommandHandler.Command = null;
            stunComplete.Invoke();
        }

        
    }
}
