using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class DodgeBehaviour : Behaviour
    {
        [Header("Dodge Behaviour")]
        public UnityEvent DodgeComplete = new();
        [SerializeField] private float dodgeSpeed;

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
            myth.Stamina.Value -= 0;
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
            Debug.Log("Activating" + myth);
            myth.isInvulnerable = true;
            movementController.SetTargetVelocity(myth.transform.forward * dodgeSpeed);
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
