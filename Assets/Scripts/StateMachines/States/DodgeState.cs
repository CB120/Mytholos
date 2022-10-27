using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class DodgeState : State
    {
        [Header("Dodge Behaviour")]
        public UnityEvent DodgeComplete = new();
        private float dodgeSpeed = 6;

        // References
        [SerializeField] private CollisionDetection movementController;
        private DodgeCommand dodgeCommand;


        [Header("SFX")]
        public GameObject dodgeSFXPrefab;
        public float timeToDestroyDodgeSFX = 1f;

        //private void Update()
        //{
            //Debug.Log($"{myth.name} used a dodge");
        //}

        protected override void OnEnable()
        {
            base.OnEnable();
            // TODO: If we don't have the stamina, we shouldn't have made it to this state
            if (myth.Stamina.Value < 1)
            {
                DodgeComplete.Invoke();
                return;
            }
            dodgeCommand = mythCommandHandler.LastCommand as DodgeCommand;
            myth.Stamina.Value -= 1;
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
            myth.IsInvulnerable = true;
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

            GameObject dodgeSFX = Instantiate(dodgeSFXPrefab, transform);
            Destroy(dodgeSFX, timeToDestroyDodgeSFX);
        }

        private void KilliFrames()
        {
            movementController.SetTargetVelocity(Vector3.zero);
            myth.IsInvulnerable = false;
            
            DodgeComplete.Invoke();
        }

    }
}
