using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class MoveBehaviour : Behaviour
    {
        public UnityEvent moveComplete = new();

        public NavMeshAgent navMeshAgent;
        [SerializeField] private Transform targetUnit;

        private void Start()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();

            if (navMeshAgent == null)
            {
                Debug.Log("There was a problem assigning " + this.gameObject.name + " to the navmesh");
            }
            else
            {
                SetDestination();
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
            if(targetUnit != null)
            {
                Vector3 targetVector = targetUnit.transform.position;
                navMeshAgent.SetDestination(targetVector);
            }
            else { Debug.Log("No Unit Selected"); }
        }
    }
}