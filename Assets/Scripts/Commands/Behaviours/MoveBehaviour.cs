using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class MoveBehaviour : Behaviour
    {
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();

        // TODO: This needs to be hooked back up
        // TODO: So does rotation
        [SerializeField] private Animator anim;
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private float speed;
        public NavMeshAgent navMeshAgent;

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
            SetDestination();
        }

        private void Update()
        {
            // Debug.Log($"{myth.name} moved. {((MoveCommand)myth.Command).CurrentMoveCommandType}");
            
            navMeshAgent.gameObject.transform.rotation = Quaternion.Slerp(navMeshAgent.gameObject.transform.rotation, NewRotation(), Time.deltaTime);

            movementController.SetTargetVelocity((navMeshAgent.steeringTarget - transform.position).normalized * speed);

            CheckDestination();
        }

        private void SetDestination()
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

        private void CheckDestination()
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

        private Quaternion NewRotation()
        {
            Vector3 facingDirection = (navMeshAgent.steeringTarget);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(facingDirection.x, 0, facingDirection.z), navMeshAgent.gameObject.transform.up);
            return lookRotation;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(navMeshAgent.steeringTarget, 0.3f);
        }
    }
}