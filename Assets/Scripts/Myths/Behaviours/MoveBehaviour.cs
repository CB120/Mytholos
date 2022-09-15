using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class MoveBehaviour : Behaviour
    {
        public UnityEvent moveComplete = new();

        [SerializeField] private Animator anim;
        public NavMeshAgent navMeshAgent;
        [SerializeField] private GameObject targetUnit;
        private bool activePath = false;

        private void Start()
        {
            navMeshAgent = myth.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.Log("There was a problem assigning " + myth.gameObject.name + " to the navmesh");
            }
        }


        private void Update()
        {
            Debug.Log($"{myth.name} moved. {((MoveCommand)myth.Command).CurrentMoveCommandType}");

            myth.Command = null;
            SetDestination();
            moveComplete.Invoke();
        }


        private void SetDestination()
        {
            //Debug.Log("CALLING SET DESTINATION");
            if (targetUnit == null)
            {
                targetUnit = myth.targetEnemy;
            }
            if(targetUnit != null)
            {
                Vector3 targetVector = targetUnit.transform.position;
                navMeshAgent.SetDestination(targetVector);
                if (anim) anim.SetBool("Walking", true);
                activePath = true;
            }
            else { Debug.Log("No Unit Selected"); }

            if (activePath)
            {
                InvokeRepeating("CheckDestination", 1f, 0.2f); // Might need to play with these values for the sake of efficiency
            }
        }

        private void CheckDestination()
        {
            navMeshAgent.gameObject.transform.rotation = Quaternion.Slerp(navMeshAgent.gameObject.transform.rotation, UpdateRotation(), Time.deltaTime);
            //Debug.Log("CALLING CHECK DESTINATION");
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.ResetPath();
                if (anim) anim.SetBool("Walking", false);
                CancelInvoke("CheckDestination");
                Debug.Log("Complete " + navMeshAgent.pathStatus);
                activePath = false;
            }
        }

        private Quaternion UpdateRotation()
        {
            Vector3 facingDirection = (navMeshAgent.steeringTarget);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(facingDirection.x, 0, facingDirection.z), navMeshAgent.gameObject.transform.up);
            Debug.Log(facingDirection.x + " " + facingDirection.z);    
            return lookRotation;
            
        }
    }
}