using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Commands;
using System.Collections.Generic;

namespace Myths.Behaviours
{
    public class MoveState : State
    {
        [Header("Move Behaviour")]
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();
        

        [SerializeField] private Animator anim;
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private float speed;
        public NavMeshAgent navMeshAgent;

        [SerializeField] private List<NavigationNode> navigationNode;


        private new void Awake()
        {
            base.Awake();
            // I fucking hate using tags, theyre so inefficient, however im just testing this solution for now & will optimize later ~ Christian
            foreach (GameObject node in GameObject.FindGameObjectsWithTag("NavigationNode"))
            {
                NavigationNode navNode;
                navNode = node.GetComponent<NavigationNode>();
                navigationNode.Add(navNode);
            }
        }

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
            if (myth.targetEnemy == null) return;
            switch (((AIMoveCommand)mythCommandHandler.Command).CurrentMoveCommandType) { 
                case AIMoveCommand.MoveCommandType.Approach:
                    ApproachEnemy();
                    Debug.Log("Approaching!");
                    break;
                case AIMoveCommand.MoveCommandType.Flee:
                    FleeEnemy();
                    Debug.Log("Fleeing!");
                    break;
                case AIMoveCommand.MoveCommandType.ApproachAttack:
                    ApproachEnemy();
                    Debug.Log("Approaching to attack!");
                    break;
                case AIMoveCommand.MoveCommandType.Support:
                    break;
                case AIMoveCommand.MoveCommandType.Idle:
                    navMeshAgent.ResetPath();
                    break;
                default:
                    Debug.Log("How the fuck did you get here?");
                break;
            }   
        }

        private void Update()
        {
             Debug.Log($"{myth.name} moved. {((AIMoveCommand)mythCommandHandler.Command).CurrentMoveCommandType}");
            
            navMeshAgent.gameObject.transform.rotation = Quaternion.Slerp(navMeshAgent.gameObject.transform.rotation, NewRotation(), Time.deltaTime * 10);

            movementController.SetTargetVelocity((navMeshAgent.steeringTarget - transform.position).normalized * speed);

            switch (((AIMoveCommand)mythCommandHandler.Command).CurrentMoveCommandType)
            {
                case AIMoveCommand.MoveCommandType.Approach:
                    ApproachEnemyDistanceCheck();
                    break;
                case AIMoveCommand.MoveCommandType.Flee:
                    killFlee();
                    break;
                case AIMoveCommand.MoveCommandType.ApproachAttack:
                    AttackRangeDistanceCheck();
                    break;
                case AIMoveCommand.MoveCommandType.Support:
                    break;
                case AIMoveCommand.MoveCommandType.Idle:
                    if (anim) anim.SetBool("Walking", false);
                    mythCommandHandler.Command = null;
                    moveComplete.Invoke();
                    break;
                default:
                    Debug.Log("How the fuck did you get here?");
                    break;
            }

           

        }

        //** Approach related function **/
        private void ApproachEnemy()
        {
            if (myth.targetEnemy == null)
            {
                mythCommandHandler.Command = null;
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
                mythCommandHandler.Command = null;
                moveComplete.Invoke();
            }
        }

        /** Flee functions **/
        private void FleeEnemy()
        {
            if(navigationNode == null)
            {
                Debug.Log("Something went wrong with the nodes, please check again");
                mythCommandHandler.Command = null;
                moveFailed.Invoke();
                return;
            }
            if (myth.targetEnemy == null)
            {
                mythCommandHandler.Command = null;
                moveFailed.Invoke();
                return;
            }
            // Should i make it the furthest node from the player or furthest from an enemy. Thing is, how do you tell what enemy?
            // Jk i know what ill do, ill have a bool on the nodes that will set to true if a player is within a certain distance of them. 
            // if this bool relates to an enemy of the player, dont move there ~~~ 1:30am solutions. watch me hate it tomorrow lmao

            navMeshAgent.SetDestination(furthestDistance().transform.position);
            if (anim) anim.SetBool("Walking", true);
        }

        private NavigationNode furthestDistance()
        {
            NavigationNode navNode = null;
            float furthestDistance = 0;
            float tempDistance;
            if(navigationNode != null)
            {
                foreach (NavigationNode node in navigationNode)
                {
                    tempDistance = Vector3.Distance(node.transform.position, myth.transform.position);
                    Debug.Log(tempDistance.ToString()); 
                    if(tempDistance > furthestDistance)
                    {
                        furthestDistance = tempDistance;
                        navNode = node;
                    }
                }
            }
            return navNode;
        }

        private void killFlee()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.ResetPath();
                if (anim) anim.SetBool("Walking", false);
                movementController.SetTargetVelocity(Vector3.zero);
                Debug.Log("Complete " + navMeshAgent.pathStatus);
                mythCommandHandler.Command = null;
                moveComplete.Invoke();
            }
        }

        /* Approach - Attack related functions */

        private void AttackRangeDistanceCheck()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.ResetPath();
                if (anim) anim.SetBool("Walking", false);
                movementController.SetTargetVelocity(Vector3.zero);
                Debug.Log("Complete " + navMeshAgent.pathStatus);
                // Pass the ability into here
                mythCommandHandler.Command = null;
                moveComplete.Invoke();
            }
        }


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