using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class DodgeState : State
    {
        [Header("Dodge Behaviour")]
        public UnityEvent DodgeComplete = new();
        private float dodgeSpeed = 6;

        // References
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animator anim;


        private void Update()
        {
            Debug.Log($"{myth.name} used a dodge");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (myth.Stamina.Value < 35)
            {
                mythCommandHandler.Command = null;
                DodgeComplete.Invoke();
                return;
            }
            myth.Stamina.Value -= 35;
            ActivateDodge();
        }

        private Vector3 decideDirection()
        {
            Vector3 direction = Vector3.zero;
            return direction;

            // Use this in the future for a better dodge?
        }
        private void ActivateDodge()
        {
            // Initialize dodge parameters 
            myth.isInvulnerable = true;
            movementController.SetTargetVelocity(myth.transform.forward * (myth.myth.agility + dodgeSpeed));
            if (anim) anim.SetBool("Walking", false);
            Invoke("KilliFrames", 0.33f);
            mythCommandHandler.Command = null;
            DodgeComplete.Invoke();
        }

        private void KilliFrames()
        {

            movementController.SetTargetVelocity(Vector3.zero);
            myth.isInvulnerable = false;
        }
    }
}
