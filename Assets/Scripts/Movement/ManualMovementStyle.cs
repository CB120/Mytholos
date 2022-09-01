using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualMovementStyle : CollisionDetection
{
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/QuickStartGuide.html
    public float mythSpeed;
    private Vector2 inputVector;
    Quaternion newRotation;
    Quaternion lastRotation;

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        inputVector = context.ReadValue<Vector2>() * mythSpeed;
        newRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y), this.transform.up);
        if (!context.canceled)
        {
            lastRotation = newRotation;
        } else if (context.canceled)
        {
            lastRotation = this.transform.rotation;
        }
    }


    protected override void SetTargetVelocity()
    {
        base.SetTargetVelocity();
        velocity = new Vector3(inputVector.x, velocity.y, inputVector.y);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lastRotation, Time.deltaTime * 8);
    }
}
