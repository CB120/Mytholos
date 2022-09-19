using Myths;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class ManualMovementStyle : MonoBehaviour
{
    // Variables 
    private Vector3 inputVector;
    //private Quaternion newRotation;
    //private Quaternion lastRotation;
    private Vector3 lastDirection;
    private Vector3 targetDirection;
    private float lerpTime = 0;
    private float targetLerpSpeed = 1f;
    private float smoothing = 0.25f;

    // Properties 


    // References
    [SerializeField] private Animator anim;
    [SerializeField] private Myth mythProperties;
    public NavMeshAgent navMeshAgent;

    /*public void Move(Vector2 input)
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
    }*/

    private void Update()
    {
        inputVector.Normalize();
        if (inputVector != lastDirection)
        {
            lerpTime = 0;
        }
        lastDirection = inputVector;
        targetDirection = Vector3.Lerp(targetDirection, inputVector, Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));

        navMeshAgent.Move(targetDirection * mythProperties.walkSpeed * Time.deltaTime);


        Vector3 lookDirection = inputVector;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
        }
        lerpTime += Time.deltaTime;

        //mythProperties.isManuallyMoving = true;
    }

    public void Move(Vector2 input, InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            inputVector = new Vector3(input.x, 0, input.y);
        } else if (context.canceled)
        {
            inputVector = Vector3.zero;
        }

        Debug.Log("Working");
        inputVector = new Vector3(input.x, 0, input.y);
    
        /*inputVector.Normalize();
        if(inputVector != lastDirection)
        {
            lerpTime = 0;
        }
        lastDirection = inputVector;
        targetDirection = Vector3.Lerp(targetDirection, inputVector, Mathf.Clamp01(lerpTime * targetLerpSpeed *(1 - 0.25f /*Can add a variable for the 0.25f*///)));

        /*navMeshAgent.Move(targetDirection * mythProperties.walkSpeed * Time.deltaTime);
        Vector3 lookDirection = inputVector;
        if(lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - 0.25f)));
        }
        lerpTime += Time.deltaTime;

        mythProperties.isManuallyMoving = true;
       */
    }


    public void Dodge(Vector2 input)
    {
        // Can either access the rigidbody and use a push force function to quickly move the myth, however this makes eddies script a bit useless
        // Can increase the speed and keep moving the myth in a direction
    }



    /*protected override void SetTargetVelocity()
    {
            base.SetTargetVelocity();
            velocity = new Vector3(inputVector.x, velocity.y, inputVector.y);
            if (mythProperties.isManuallyMoving)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lastRotation, Time.deltaTime * 8);
            }
        }*/
}
