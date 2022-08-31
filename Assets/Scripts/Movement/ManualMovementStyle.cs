using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualMovementStyle : CollisionDetection
{
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/QuickStartGuide.html
    public float mythSpeed;
    private Vector2 inputVector;

    public void Move(InputAction.CallbackContext context)
    {
        //Debug.Log("Moving!" + context);
        inputVector = context.ReadValue<Vector2>() * mythSpeed;
    }


    protected override void SetTargetVelocity()
    {
        base.SetTargetVelocity();
        velocity = new Vector3(inputVector.x, velocity.y, inputVector.y);
    }
}
