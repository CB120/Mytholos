using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class MoveState : State
    {
        [Header("Manual Move Behaviour")]
        // Movement Properties

        [SerializeField] private float speedHandicap = 1.2f;
        private float acceleration = 10;
        private float moveSpeed = 0;

        // References & Events
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();

        [SerializeField] private CollisionDetection movementController;

        private MoveCommand moveCommand;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            moveCommand = mythCommandHandler.LastCommand as MoveCommand;

            if (moveCommand == null)
            {
                Debug.LogWarning("There was a problem with finding the manualMovementCommand on the Myth's Command Handler.");
                moveFailed.Invoke();
            }

            if(movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                moveFailed.Invoke();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Charge")){
                anim.ResetTrigger("Charge");
                Debug.Log("Attempting to reset charge anim");
            }
        }

        private float speedValue()
        {
            moveSpeed += (acceleration - (myth.myth.size * 2)) * (Time.deltaTime * 1.5f);
           
            //Debug.Log(myth.myth.size);
            moveSpeed = Mathf.Clamp(moveSpeed, 0, (myth.walkSpeed * 5) * speedHandicap);
            //Debug.Log(moveSpeed);
            return moveSpeed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            movementController.SetTargetVelocity(Vector3.zero);
            moveSpeed = 0;
            if (anim) anim.SetBool("Walking", false);
        }



        private void Update()
        {
            if (!myth.isInvulnerable)
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
                    //anim.SetFloat("Speed", walkSpeed);
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
