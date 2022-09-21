using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Commands;
using System.Collections.Generic;

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

        private void OnEnable()
        {
           
            if(((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Approach)
            {
                ApproachEnemy();
                Debug.Log("Approaching!");
            }
            if (((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Flee)
            {
                FleeEnemy();
                Debug.Log("Fleeing!");
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
            if (((MoveCommand)myth.Command).CurrentMoveCommandType == MoveCommand.MoveCommandType.Flee)
            {
                killFlee();
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
        private void FleeEnemy()
        {
            if(navigationNode == null)
            {
                Debug.Log("Something went wrong with the nodes, please check again");
                myth.Command = null;
                moveFailed.Invoke();
                return;
            }
            if (myth.targetEnemy == null)
            {
                myth.Command = null;
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

        // Debug
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(navMeshAgent.steeringTarget, 0.3f);
        }
    }
}