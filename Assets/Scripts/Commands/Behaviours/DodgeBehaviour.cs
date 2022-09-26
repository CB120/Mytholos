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
        

        // References
        //public NavMeshAgent navMeshAgent;
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animator anim;
        [SerializeField] private float dodgeSpeed;

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
            Debug.Log(myth.Stamina.Value);
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
