using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class MoveState : State
    {
        [Header("Manual Move Behaviour")]
        // Movement Properties
        private float acceleration = 10;
        private float moveSpeed = 0;
        private float maxSpeed = 0;

        // References & Events
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();

        [SerializeField] private CollisionDetection movementController;

        private MoveCommand moveCommand;

        protected override void OnEnable()
        {
            base.OnEnable();

            moveCommand = mythCommandHandler.LastCommand as MoveCommand;
            maxSpeed = myth.walkSpeed;

            if (moveCommand == null)
            {
                Debug.LogWarning("There was a problem with finding the manualMovementCommand on the Myth's Command Handler.");
                moveFailed.Invoke();
            }

            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                moveFailed.Invoke();
            }
        }

        private float speedValue()
        {
            if (moveSpeed < maxSpeed)
                moveSpeed += (acceleration - (myth.myth.size)) * (Time.deltaTime); // This is for acceleration? but like ? wot
            else
                moveSpeed = maxSpeed;
            return moveSpeed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            movementController.SetTargetVelocity(Vector3.zero);
            moveSpeed = 0;

            if (anim)
            {
                anim.SetBool("Walking", false);
                anim.speed = 1.0f;
            }
        }

        private void Update()
        {
            if (!myth.IsInvulnerable)
            {
                var inputVector = new Vector3(
                    moveCommand.input.x,
                    0,
                    moveCommand.input.y
                );

                if (inputVector == Vector3.zero)
                {
                    movementController.SetTargetVelocity(Vector3.zero);
                    moveSpeed = 0;
                    if (anim)
                    {
                        anim.SetBool("Walking", false);
                        anim.speed = 1.0f;
                    }

                    // TODO: I don't like having to do this...
                    mythCommandHandler.DemoteCurrentCommand();
                    moveComplete.Invoke();
                    return;
                }

                inputVector.Normalize();

                movementController.SetTargetVelocity(inputVector * speedValue());

                if (anim)
                {
                    anim.SetBool("Walking", true);
                    float walkSpeed = (inputVector * moveSpeed).magnitude;
                    anim.SetFloat("MoveTween", walkSpeed / 10.0f);
                    anim.speed = walkSpeed;
                }

                Vector3 lookDirection = inputVector;
                if (lookDirection != Vector3.zero)
                {
                    myth.gameObject.transform.rotation = Quaternion.Slerp(myth.gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 9);
                }
            }
        }
    }
}
