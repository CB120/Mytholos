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
            else
            {
                SetDestination();
                Debug.Log("Destination set");
            }
        }


        private void Update()
        {
            Debug.Log($"{myth.name} moved. {((MoveCommand)myth.Command).CurrentMoveCommandType}");

            myth.Command = null;

            moveComplete.Invoke();
        }


        private void SetDestination()
        {
            if(targetUnit == null)
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
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.ResetPath();
                if (anim) anim.SetBool("Walking", false);
                CancelInvoke("CheckDestination");
                moveComplete.Invoke();
                Debug.Log("Complete " + navMeshAgent.pathStatus);
                activePath = false;
            }
        }
    }
}