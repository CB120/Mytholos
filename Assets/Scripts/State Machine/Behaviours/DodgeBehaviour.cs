using Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Myths.Behaviours
{
    public class DodgeBehaviour : Behaviour
    {
        [Header("Dodge Behaviour")]
        public UnityEvent DodgeComplete = new();
        private float dodgeSpeed = 6;

        // References
        [SerializeField] private CollisionDetection movementController;
        private DodgeCommand dodgeCommand;

        private void Update()
        {
            //Debug.Log($"{myth.name} used a dodge");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (myth.Stamina.Value < 35)
            {
                mythCommandHandler.Command = null;
                DodgeComplete.Invoke();
                return;
            }
            dodgeCommand = mythCommandHandler.Command as DodgeCommand;
            myth.Stamina.Value -= 35;
            ActivateDodge();
        }

        private /*Vector3*/ void DecideDirection()
        {
            //Vector3 direction = Vector3.zero;
            //return direction;

            var inputVector = new Vector3(
                dodgeCommand.input.x,
                0,
                dodgeCommand.input.y
            );

            myth.gameObject.transform.rotation = Quaternion.LookRotation(inputVector);

            // Use this in the future for a better dodge? // Okey
        }
        private void ActivateDodge()
        {
            // Initialize dodge parameters 
            myth.isInvulnerable = true;
            if (dodgeCommand != null)
                DecideDirection();
            movementController.SetTargetVelocity(myth.transform.forward * (myth.myth.agility + dodgeSpeed));
            if (anim)
            {
                anim.SetTrigger("Dodge");
                anim.SetBool("Walking", false);
                anim.speed = 1.0f;
            }
            Invoke("KilliFrames", 0.33f);
            mythCommandHandler.Command = null;
            DodgeComplete.Invoke();
        }

        private void KilliFrames()
        {

            movementController.SetTargetVelocity(Vector3.zero);
            myth.isInvulnerable = false;
        }
    }
}
