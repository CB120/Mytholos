using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class MoveState : State
    {
        [Header("Manual Move Behaviour")]
        // Movement Properties
        private Vector3 lastDirection;
        private Vector3 targetDirection;

        [SerializeField] private float lerpTime = 0;
        [SerializeField] private float targetLerpSpeed = 0.1f;
        [SerializeField]private float smoothing = 0.1f;
        [SerializeField] private float speedHandicap = 1.2f;
        private float acceleration = 4;
        private float moveSpeed = 0;

        // References & Events
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();

        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animator anim;

        private MoveCommand moveCommand;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            moveCommand = mythCommandHandler.Command as MoveCommand;

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

        }

        private float speedValue()
        {
            moveSpeed += (acceleration - (myth.myth.size * 2)) * (Time.deltaTime * 1.5f);
           
            //Debug.Log(myth.myth.size);
            moveSpeed = Mathf.Clamp(moveSpeed, 0, myth.walkSpeed * speedHandicap);
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
                    mythCommandHandler.Command = null;
                    movementController.SetTargetVelocity(Vector3.zero);
                    moveSpeed = 0;
                    if (anim) anim.SetBool("Walking", false);
                    moveComplete.Invoke();
                    return;
                }

                inputVector.Normalize();

                if (inputVector != lastDirection)
                {
                    lerpTime = 0;
                }

                lastDirection = inputVector;

                targetDirection = Vector3.Lerp(targetDirection, inputVector,
                    Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing))); // Only to be used with some sort of ice state / movement

                if (anim) anim.SetBool("Walking", true);

                //movementController.SetTargetVelocity(inputVector * (myth.myth.agility * speedHandicap));
                
                movementController.SetTargetVelocity(inputVector * speedValue());
                
                
                //movementController.SetTargetVelocity(targetDirection * myth.myth.agility * speedHandicap); USING THIS IS LIKE WALKING ON ICE (6 TARGET LERP SPEED, 0.45 SMOOTHING)

                Vector3 lookDirection = inputVector;
                myth.lastInputDirection = inputVector;
                if (lookDirection != Vector3.zero)
                {
                    myth.gameObject.transform.rotation = Quaternion.Slerp(myth.gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 9);
                }

                lerpTime += Time.deltaTime;
            }
        }
    }
}
