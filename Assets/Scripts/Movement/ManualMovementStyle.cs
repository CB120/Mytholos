using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualMovementStyle : MovementStyleManager
{
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/QuickStartGuide.html
    public Rigidbody rb;
    
    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("Moving!" + context);
        Vector2 inputVector = context.ReadValue<Vector2>();
        float speed = 2.0f;
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
}
