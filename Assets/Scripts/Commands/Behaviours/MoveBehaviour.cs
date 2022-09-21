using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Commands;

namespace Myths.Behaviours
{
    public class MoveBehaviour : Behaviour
    {
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();

        [SerializeField] private Animator anim;
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private float speed;
        public NavMeshAgent navMeshAgent;

        //[SerializeField] private 

        private void Start()
        {
            navMeshAgent = myth.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.Log("There was a problem assigning " + myth.gameObject.name + " to the navmesh");
            }
        }

        private void OnEnable()
        {
            if(((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Approach)
            {
                ApproachEnemy();
                Debug.Log("Approaching!");
            }
            if (((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Flee)
            {
                
                Debug.Log("Approaching!");
            }

        }

        private void Update()
        {
             Debug.Log($"{myth.name} moved. {((MoveCommand)myth.Command).CurrentMoveCommandType}");
            
            navMeshAgent.gameObject.transform.rotation = Quaternion.Slerp(navMeshAgent.gameObject.transform.rotation, NewRotation(), Time.deltaTime);

            movementController.SetTargetVelocity((navMeshAgent.steeringTarget - transform.position).normalized * speed);

            if (((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Approach)
            {
                ApproachEnemyDistanceCheck();
            }
         
        }

        //** Approach related function **/
        private void ApproachEnemy()
        {
            if (myth.targetEnemy == null)
            {
                myth.Command = null;
                moveFailed.Invoke();
                return;
            }
            
            navMeshAgent.SetDestination(myth.targetEnemy.transform.position);
            
            if (anim) anim.SetBool("Walking", true);
        }

        private void ApproachEnemyDistanceCheck()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.ResetPath();
                if (anim) anim.SetBool("Walking", false);
                movementController.SetTargetVelocity(Vector3.zero);
                Debug.Log("Complete " + navMeshAgent.pathStatus);
                myth.Command = null;
                moveComplete.Invoke();
            }
        }

        /** Flee functions **/


        private Quaternion NewRotation()
        {
            Vector3 facingDirection = (navMeshAgent.steeringTarget);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(facingDirection.x, 0, facingDirection.z), navMeshAgent.gameObject.transform.up);
            return lookRotation;
        }

        // Debug
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(navMeshAgent.steeringTarget, 0.3f);
        }
    }
}