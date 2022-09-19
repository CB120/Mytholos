using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class DodgeBehaviour : Behaviour
    {
        public UnityEvent DodgeComplete = new();
        public NavMeshAgent navMeshAgent;
        private float speed = 0;
        private float acceleration = 0;

        private void Update()
        {
            Debug.Log($"{myth.name} used a dodge");

            myth.Command = null;
            Debug.Log("Dodging");
            ActivateDodge();
            DodgeComplete.Invoke();
        }

        private void ActivateDodge()
        {
            speed = navMeshAgent.speed;
            acceleration = navMeshAgent.acceleration;
            myth.isInvulnerable = true;
            navMeshAgent.speed = 200;
            navMeshAgent.acceleration = 200;
            navMeshAgent.Move(new Vector3(90,0,0));
            Invoke("KillDodge", 1.2f);
        }

        private void KillDodge()
        {
            myth.isInvulnerable = false;
            navMeshAgent.speed = speed;
            navMeshAgent.acceleration = acceleration;

        }
    }
}
