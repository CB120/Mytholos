using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class FreezeState : State
    {
        // References & Events
        public UnityEvent freezeComplete = new();
        public UnityEvent freezeFailed = new();
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animation anim;
        private MoveCommand moveCommand;

        // Freeze Movement p r o p e r t i e s 
        private float lerpTime = 0;
        private float targetLerpSpeed = 6f;
        private float smoothing = 0.45f;
        // Movement Properties
        private Vector3 lastDirection;
        private Vector3 targetDirection;
        private float deceleration = 1;

        protected override void OnEnable()
        {
            base.OnEnable();

            moveCommand = mythCommandHandler.LastCommand as MoveCommand;

            if (moveCommand == null)
            {
                Debug.LogWarning("There was a problem with finding the manualMovementCommand on the Myth's Command Handler.");
                freezeFailed.Invoke();
            }

            if (movementController == null)
            {
                Debug.LogWarning("There was a problem with finding the movementController (CollisionDetection Physics). Please re-assign it in the inspector.");
                freezeFailed.Invoke();
                return;
            }

            // Pseudocode : while the myth is on the ice debris, count to x. If the count reaches x, complete this state & move to stun

        }

        private void Update()
        {
            deceleration -= Time.deltaTime / 60;
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
                    //if (anim) anim.SetBool("Walking", false);
                    //freezeComplete.Invoke();
                }

                inputVector.Normalize();

                if (inputVector != lastDirection)
                {
                    lerpTime = 0;
                }

                lastDirection = inputVector;

                targetDirection = Vector3.Lerp(targetDirection, inputVector,
                    Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing))); // Only to be used with some sort of ice state / movement

                //if (anim) anim.SetBool("Walking", true);


                movementController.SetTargetVelocity(targetDirection * myth.myth.agility * deceleration);

                Vector3 lookDirection = inputVector;
                myth.lastInputDirection = inputVector;
                if (lookDirection != Vector3.zero)
                {
                    myth.gameObject.transform.rotation = Quaternion.Slerp(myth.gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 9);
                }

                lerpTime += Time.deltaTime;
            }
        }


        private void moveToStun()
        {
            freezeComplete.Invoke();
        }
    }
}
