using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    // Create a scriptable object for this
    public class DodgeBehaviour : Behaviour
    {
        [Header("Dodge Behaviour")]
        public UnityEvent DodgeComplete = new();
        public NavMeshAgent navMeshAgent;
        private Vector3 inputDirection;


        private void Update()
        {
            Debug.Log($"{myth.name} used a dodge");


            //ActivateDodge();
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
            //inputDirection = new Vector3(myth.lastInputDirection.x, 0, myth.lastInputDirection.y);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(myth.lastInputDirection.x, 0, myth.lastInputDirection.y), myth.transform.up);
            inputDirection = lookRotation * Vector3.forward;
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
            myth.transform.position = transform.position + (dodgeDirection * 1.75f);
            //Debug.Log(dodgeDirection);
            KillDodge();
            Invoke("KilliFrames", 0.33f);
        }

        private void KillDodge()
        {
            //navMeshAgent.ResetPath();
            //myth.transform.position = myth.transform.position;
            mythCommandHandler.Command = null;
            DodgeComplete.Invoke();
        }

        private void KilliFrames()
        {
            myth.isInvulnerable = false;
        }
    }
}
