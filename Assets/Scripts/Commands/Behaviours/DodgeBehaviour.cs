using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    // Create a scriptable object for this
    public class DodgeBehaviour : Behaviour
    {
        public UnityEvent DodgeComplete = new();
        public NavMeshAgent navMeshAgent;
        private float speed = 0;
        private float acceleration = 0;
        private Vector3 inputDirection;


        private void Update()
        {
            Debug.Log($"{myth.name} used a dodge");


            //ActivateDodge();
        }

        private void OnEnable()
        {
            inputDirection = new Vector3(myth.lastInputDirection.x, 0, myth.lastInputDirection.y);
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
        }
        private void ActivateDodge(Vector3 dodgeDirection)
        {
            // Initialize dodge parameters 
            speed = navMeshAgent.speed;
            acceleration = navMeshAgent.acceleration;
            myth.isInvulnerable = true;
            navMeshAgent.speed = 200;
            navMeshAgent.acceleration = 200;
            navMeshAgent.stoppingDistance = 0;
            
            Debug.Log(dodgeDirection);
            //navMeshAgent.SetDestination(myth.gameObject.transform.position + dodgeDirection * 1.5f);
            Invoke("KillDodge", 0.19f);
            Invoke("KilliFrames", 0.33f);
        }

        private void KillDodge()
        {
            navMeshAgent.ResetPath();
            myth.gameObject.transform.position = myth.transform.position;
            navMeshAgent.speed = speed;
            navMeshAgent.acceleration = acceleration;
            navMeshAgent.stoppingDistance = 3;
            myth.Command = null;
            DodgeComplete.Invoke();
        }

        private void KilliFrames()
        {
            myth.isInvulnerable = false;
        }
    }
}
