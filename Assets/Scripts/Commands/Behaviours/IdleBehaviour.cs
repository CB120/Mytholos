using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class IdleBehaviour : Behaviour
    {
        public UnityEvent abilityCommandReceived = new();
        public UnityEvent moveCommandReceived = new();
        public UnityEvent manualMoveCommandReceived = new();

        [SerializeField] private Animator anim;
        public NavMeshAgent navMeshAgent;

        /* When the myth is set to Auto (Currently known as idle behaviour), it will go through a list of things to do.
        - It will first check to see if it is low health, if so it will fuck off to the other side of the map. 
        - It will next check if its team mate is low health. If so, heal it if possible. 
        - Next, it will check if there are any enemies that are low health (less than 15% HP). If so, and it has atleast 50% stamina, it will attack that enemy
        - If none of these conditions are satisfied, it will move around till its stamina is atleast 80%, then it will opt for a ranged attack
        - If its stamina is not atleast 80%, it will roam around the map (far from enemies) waiting for the regen. 


        This brings me to my next stage of the AI. Personally, i believe not everyone wants their AI to play like this. Similarly to fifa, I want
        to be able to set the style my AI will take on when its making its own decisions. I may want it to be super defensive, or i may want it 
        to be super aggresive and not worry about stamina or its own health. This will be something for me to work out later on. 

        */

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
            // TODO: Move these to an AnyStateTransition
            if (myth.Command == null) return;

            if (myth.Command is AbilityCommand)
            {
                abilityCommandReceived.Invoke();
            }

            if (myth.Command is MoveCommand)
            {
                moveCommandReceived.Invoke();
            }

            if (myth.Command is ManualMoveCommand)
            {
                manualMoveCommandReceived.Invoke();
            }
        }

        private void GoalOrientedDecision()
        {
            // This is where a decision is made for what to do on auto-pilot based on the goal to be achieved
        }


        private void LowHealthResponse()
        {
            // This is the response to low health
        }

        private void TeamSupportResponse()
        {
            // This is the response to help your teammates health 
        }

        private void ExecutionResponse()
        {
            // This is the reponse to execute a critical-health enemy
        }

        private void SniperResponse()
        {
            // This is the response to do chip damage from afar
        }

        private void StaminaRecoveryResponse()
        {
            // This is the response to recover stamina and line up the next shot on the target
        }

    }
}