using UnityEngine;

// TODO: Rename so that actual function is more clear
public class CollisionDetection : MonoBehaviour
{
    protected enum HitDirection { Left, Right, Down, Up, Back, Front } // X-, X+, Y-, Y+, Z-, Z+

    [SerializeField] protected Vector3 gravity = new Vector3(0.0f, 0.0f, 0.0f);        // Default force that is applied every FixedUpdate
    [SerializeField] protected float steepestSlope = 0.65f;                             // The maximum angled slope we can walk up (this is the y component of the face's normal vector)
    [SerializeField] protected bool groundOnHorizontalMovement = false;                 // Optional (not super important?), if true it sets our ground normal on horizontal movement as well as downward ones
    [SerializeField] protected bool hugGround = false;                                  // Optional, object will 'magnetically' hug the ground if possible when moving down a slope or off a ledge
    [SerializeField] protected float hugGroundDistance = 5.0f;                          // Distance from ground that this object will 'magnetically' reconnect to the ground below them if they were previously grounded
    
    [Header("Sliding")]
    // [SerializeField] private float targetLerpSpeed = 6f;
    // [SerializeField] private float smoothing = 0.45f;
    [Tooltip("Roughly the number of seconds to reach top speed while sliding.")]
    [SerializeField] private float slideyness;
    private Vector3 lastInputVelocity;
    private Vector3 targetVelocity;
    private float lerpTime;
    [SerializeField] bool isSliding;

    public bool IsSliding
    {
        get => isSliding;
        set
        {
            Debug.Log(value);
            isSliding = value;
            
            if (isSliding == false)
                velocity = Vector3.zero;
        }
    }


    protected float minimumDistance = 0.001f;
    protected float collisionBuffer = 0.01f;

    [SerializeField] protected Vector3 velocity;
    protected bool wasGrounded;
    protected bool isGrounded;
    protected new Rigidbody rigidbody;
    protected RaycastHit hitBuffer = new RaycastHit();
    protected Vector3 groundNormal = Vector3.up;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        // Set and reset working variables
        velocity += gravity * Time.deltaTime;
        isGrounded = false;
        Vector3 deltaPosition = velocity * Time.deltaTime;

        // Perform a movement along each axis individually, handling any collisions that may occur
        Move(deltaPosition.y * Vector3.up, HitDirection.Up);
        Move(deltaPosition.x * new Vector3(groundNormal.y, -groundNormal.x, 0.0f).normalized, HitDirection.Right);
        Move(deltaPosition.z * new Vector3(0.0f, -groundNormal.z, groundNormal.y).normalized, HitDirection.Front);

        // Try to magnetically hug the ground
        if (wasGrounded && hugGround) Move(Vector3.down * hugGroundDistance * Time.deltaTime, HitDirection.Up, true);

        // Reset last known normal if no longer on the ground
        if (!isGrounded) groundNormal = Vector3.up;

        // Make record of groundedness (for hugging the ground)
        wasGrounded = isGrounded;
        
        if(rigidbody.velocity.magnitude > 0) // Quick fix
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    protected virtual void Move(Vector3 move, HitDirection direction, bool onlyMoveIfCollides = false)
    {
        bool collisionOccurred = false;
        float distance = move.magnitude;

        if (rigidbody.SweepTest(move, out hitBuffer, distance + collisionBuffer, QueryTriggerInteraction.Ignore))
        {
            collisionOccurred = true;

            // Clamp momentum based on normal of face you have collided with
            switch (direction)
            {
                case HitDirection.Right:
                    if (hitBuffer.normal.y <= steepestSlope && hitBuffer.normal.y >= -steepestSlope)
                    {
                        if (hitBuffer.normal.x > 0.0f) HitWall(hitBuffer, HitDirection.Right);
                        else if (hitBuffer.normal.x < 0.0f) HitWall(hitBuffer, HitDirection.Left);
                        else if (groundOnHorizontalMovement)
                        {
                            isGrounded = true;
                            groundNormal = hitBuffer.normal;
                        }
                    }
                    break;
                case HitDirection.Up:
                    if (hitBuffer.normal.y > 0.0f) HitWall(hitBuffer, HitDirection.Up);
                    else if (hitBuffer.normal.y <= 0.0f) HitWall(hitBuffer, HitDirection.Down);
                    break;
                case HitDirection.Front:
                    if (hitBuffer.normal.y <= steepestSlope && hitBuffer.normal.y >= -steepestSlope)
                    {
                        if (hitBuffer.normal.z > 0.0f) HitWall(hitBuffer, HitDirection.Front);
                        else if (hitBuffer.normal.z < 0.0f) HitWall(hitBuffer, HitDirection.Back);
                        else if (groundOnHorizontalMovement)
                        {
                            isGrounded = true;
                            groundNormal = hitBuffer.normal;
                        }
                    }
                    break;
                default:
                    break;
            }

            Vector3 normal = hitBuffer.normal;

            if (hitBuffer.collider.gameObject.tag == "Untagged")
            {
                // Subtract distance you would clip into object from velocity
                float projection = Vector3.Dot(velocity, normal);
                if (projection > 0.0f) velocity -= projection * normal;

                // Use whichever calculated distance is more conservative
                float modifiedDistance = hitBuffer.distance - collisionBuffer;
                if (distance > modifiedDistance) distance = modifiedDistance;
            }
        }

        // Apply calculated physics
        if (!onlyMoveIfCollides || (onlyMoveIfCollides && collisionOccurred)) rigidbody.position += move.normalized * distance;
    }

    private float maxMag;

    public void SetTargetVelocity(Vector3 velocity)
    {
        if (IsSliding)
        {
            // var t = Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing));
            //
            // targetVelocity = Vector3.Lerp(targetVelocity, velocity, t);
            //
            // if (velocity != lastInputVelocity)
            // {
            //     lerpTime = 0;
            // }
            //
            // lastInputVelocity = velocity;
            //
            // lerpTime += Time.deltaTime;
            //
            // this.velocity = targetVelocity;

            if (velocity.magnitude > maxMag)
            {
                maxMag = velocity.magnitude;
            }

            this.velocity = Vector3.ClampMagnitude(this.velocity + velocity / slideyness * Time.deltaTime, maxMag);
        }
        else
        {
            this.velocity = velocity;
        }
    }

    // Handles logic for hitting another collider. 'direction' is the normal of the face we've collided with.
    // Also exists to be overriden by child classes to extend/modify collision behaviour. 
    protected virtual void HitWall(RaycastHit hit, HitDirection direction)
    {
        // Cull momentum (and 'ground' this object, if neccesary) based on the normal of the face we're colliding with
        if (hit.collider.gameObject.tag == "Untagged") // Currently set to handle collisions with any untagged objects (of course, collisions only occur based on the physics layers matrix)
        {
            switch (direction)
            {
                case HitDirection.Left:
                    velocity = new Vector3(Mathf.Min(velocity.x, 0.0f), velocity.y, velocity.z);
                    break;
                case HitDirection.Right:
                    velocity = new Vector3(Mathf.Max(velocity.x, 0.0f), velocity.y, velocity.z);
                    break;
                //case HitDirection.Down: // Commented so that object doesn't bounce off of ceilings, but will instead stay afloat momentarily
                //    velocity = new Vector3(velocity.x, Mathf.Min(velocity.y, 0.0f), velocity.z);
                //    break;
                case HitDirection.Up:
                    velocity = new Vector3(velocity.x, Mathf.Max(velocity.y, 0.0f), velocity.z);
                    isGrounded = true;
                    if (hit.normal.y > steepestSlope) groundNormal = hit.normal; // If slope is not too steep, set our grounded normal to match, so that we walk in-line with the slope
                    else groundNormal = Vector3.up; // If slope is too steep, we reset our grounded normal, so that we can only walk away from the slope, not up it 
                    break;
                case HitDirection.Back:
                    velocity = new Vector3(velocity.x, velocity.y, Mathf.Min(velocity.z, 0.0f));
                    break;
                case HitDirection.Front:
                    velocity = new Vector3(velocity.x, velocity.y, Mathf.Max(velocity.z, 0.0f));
                    break;
                default:
                    break;
            }
        }
    }
}
