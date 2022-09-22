using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class ManualMoveBehaviour : Behaviour
    {
        // Movement Properties
        private Vector3 lastDirection;
        private Vector3 targetDirection;
        private float lerpTime = 0;
        private float targetLerpSpeed = 1f;
        private float smoothing = 0.1f;
        [SerializeField] private float speed;

        // References & Events
        public UnityEvent moveComplete = new();
        public UnityEvent moveFailed = new();
        [SerializeField] private CollisionDetection movementController;
        [SerializeField] private Animator anim;

        private ManualMoveCommand manualMoveCommand;

        private void OnEnable()
        {
            manualMoveCommand = mythCommandHandler.Command as ManualMoveCommand;

            if (manualMoveCommand == null)
            {
                // TODO: Unhelpful
                Debug.LogWarning("I'm not sure how you got here?");
                moveFailed.Invoke();
            }
        }

        private void Update()
        {
            if (!myth.isInvulnerable)
            {
                var inputVector = new Vector3(
                    manualMoveCommand.input.x,
                    0,
                    manualMoveCommand.input.y
                );

                if (inputVector == Vector3.zero)
                {
                    mythCommandHandler.Command = null;
                    movementController.SetTargetVelocity(Vector3.zero);
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
                    Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
                if (anim) anim.SetBool("Walking", true);
                movementController.SetTargetVelocity(inputVector * speed);


                Vector3 lookDirection = inputVector;
                myth.lastInputDirection = inputVector;
                if (lookDirection != Vector3.zero)
                {
                    myth.gameObject.transform.rotation = Quaternion.Slerp(myth.gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 8);
                    //myth.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection),
                    //Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
                }

                lerpTime += Time.deltaTime;

                //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lastRotation, Time.deltaTime * 8);
            }
        }
    }
}
