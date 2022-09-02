using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualMovementStyle : CollisionDetection
{
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/QuickStartGuide.html
    public float mythSpeed;
    private Vector2 inputVector;
    private Quaternion newRotation;
    private Quaternion lastRotation;

    // Animation shit 
    [SerializeField] private Animator anim;

    public void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>() * mythSpeed;
        newRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y), this.transform.up);
        if (!context.canceled)
        {
            anim.SetBool("Walking", true);
            lastRotation = newRotation;
        } else if (context.canceled)
        {
            anim.SetBool("Walking", false);
            lastRotation = this.transform.rotation;
        }
    }

    public void AttackExample(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("attack");
        }
    }

    protected override void SetTargetVelocity()
    {
        base.SetTargetVelocity();
        velocity = new Vector3(inputVector.x, velocity.y, inputVector.y);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lastRotation, Time.deltaTime * 8);
    }
}
