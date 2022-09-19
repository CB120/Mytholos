using Myths;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualMovementStyle : CollisionDetection
{
    // Variables 
    private Vector2 inputVector;
    private Quaternion newRotation;
    private Quaternion lastRotation;

    // Properties 


    // References
    [SerializeField] private Animator anim;
    [SerializeField] private Myth mythProperties;

    public void Move(Vector2 input)
    {
            inputVector = input * mythProperties.walkSpeed;
            mythProperties.isManuallyMoving = true;
            if (inputVector != Vector2.zero)
            {
                newRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y), this.transform.up);
                if (anim) anim.SetBool("Walking", true);
                lastRotation = newRotation;
            }
            else
            {
                mythProperties.isManuallyMoving = false;
                if (anim) anim.SetBool("Walking", false);
                lastRotation = this.transform.rotation;
            }
    }

    // TODO: Unused. Functionality needs to be bound to the PerformAbilityBehaviour.
    public void AttackExample()
    {
        if (anim) anim.SetTrigger("attack");
    }


    public void Dodge(Vector2 input)
    {
        // Can either access the rigidbody and use a push force function to quickly move the myth, however this makes eddies script a bit useless
        // Can increase the speed and keep moving the myth in a direction
    }



    protected override void SetTargetVelocity()
    {
            base.SetTargetVelocity();
            velocity = new Vector3(inputVector.x, velocity.y, inputVector.y);
            if (mythProperties.isManuallyMoving)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lastRotation, Time.deltaTime * 8);
            }
        }
    }
