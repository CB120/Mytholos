using UnityEngine;
using UnityEngine.AI;
namespace Myths.Behaviours
{
    public class IdleBehaviour : Behaviour
    {
        public NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = myth.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.Log("There was a problem assigning " + myth.gameObject.name + " to the navmesh");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            navMeshAgent.ResetPath();
            //Debug.Log("Idling");
        }
    }
}