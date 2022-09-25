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
        private Vector3 inputDirection;

        // References
        public NavMeshAgent navMeshAgent;
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
            Debug.Log(myth.Stamina.Value);
            //inputDirection = new Vector3(myth.lastInputDirection.x, 0, myth.lastInputDirection.y);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(myth.lastInputDirection.x, 0, myth.lastInputDirection.y), myth.transform.up);
            inputDirection = lookRotation * Vector3.one;
            if (inputDirection != null && inputDirection != Vector3.zero)
            {
                ActivateDodge(inputDirection);
            }
            else
            {
                //ActivateDodge(decideDirection());
            }
        }

        private Vector3 decideDirection()
        {
            Vector3 direction = Vector3.zero;
            return direction;

            // Use this in the future for a better dodge?
        }
        private void ActivateDodge(Vector3 dodgeDirection)
        {
            // Initialize dodge parameters 
            myth.isInvulnerable = true;
            movementController.SetTargetVelocity(dodgeDirection * 10f);
            //myth.transform.position = transform.position + (dodgeDirection * 1.75f);
            if (anim) anim.SetBool("Walking", false);
            //Debug.Log(dodgeDirection);
            //navMeshAgent.ResetPath();
            //navMeshAgent.gameObject.transform.position = navMeshAgent.gameObject.transform.position;
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
