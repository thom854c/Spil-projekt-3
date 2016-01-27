using System.Security.Policy;
using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    public string testogstuff;
    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangant = Mathf.Tan(75f*Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get { return velocity; } }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters {get { return overrideParameters ?? DefaultParameters; }}
    public GameObject StandingOn { get; private set; }

    public bool CanJump
    {
        get
        {
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpAnywhere)
                return jumpIn < 0;
            
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpOnGround)
                return State.IsGrounded;

            return false;
        }
    }


    private Vector2 velocity;
    private Transform myTransform;
    private Vector3 localScale;
    private BoxCollider2D boxCollider;
    private ControllerParameters2D overrideParameters;
    private float jumpIn;
    

    private Vector3
        raycastTopLeft,
        raycastBottomRight,
        raycastBottomLeft;


    private float
        verticalDistanceBetweenRays,
        horizontalDistanceBetweenRays;

    // AddOn Pointing Variables
    private Vector2 pointDirection;
    public enum PointBehaviour
    {
        CanPaPDoubleJump,
        CanPaPAnywhere,
        CanPaPOnGround,
        CantPush,
        CantPointandPush
    }


    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();
        myTransform = transform;
        localScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = boxCollider.size.x*Mathf.Abs(transform.localScale.x) - (2*SkinWidth);
        horizontalDistanceBetweenRays = colliderWidth/(TotalVerticalRays - 1);

        var colliderHeight = boxCollider.size.y*Mathf.Abs(transform.localScale.y) - (2*SkinWidth);
        verticalDistanceBetweenRays = colliderHeight/(TotalHorizontalRays - 1);
    }

    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    public void SetForce(Vector2 force)
    {
        velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
        velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        velocity.y = y;
    }

    public void Jump()
    {
        // TODO: Moving Platforms
        AddForce(new Vector2(0, Parameters.JumpMagnitude));
        jumpIn = Parameters.JumpFrequency;
    }

    public void LateUpdate()
    {
        jumpIn -= Time.deltaTime;
        velocity.y += Parameters.Gravity*Time.deltaTime;
        Move(Velocity * Time.deltaTime); 
    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            //HandlePlatforms();
            CalculateRayOrigins();

            //if (deltaMovement.y < 0 && wasGrounded) HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > .001f)
            {
                MoveHorizontally(ref deltaMovement);
            }

            MoveVertically(ref deltaMovement);
        }

        myTransform.Translate(deltaMovement, Space.World);

        //TODO: Moving platforms


        if (Time.deltaTime > 0)
        {
            velocity = deltaMovement/Time.deltaTime;
        }

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        /*
        if (State.isMovingUpSlope)
        {
            velocity.y = 0;
        }
        */
    }

    /*
    private void HandlePlatforms()
    {
        
    }
    */
    
    private void CalculateRayOrigins()
    {
        var size = new Vector2(boxCollider.size.x*Mathf.Abs(localScale.x), boxCollider.size.y*Mathf.Abs(localScale.y))/2;
        var center = new Vector2(boxCollider.offset.x*localScale.x, boxCollider.offset.y*localScale.y);

        raycastTopLeft = myTransform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        raycastBottomRight = myTransform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        raycastBottomLeft = myTransform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);


    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? raycastBottomRight : raycastBottomLeft;

        for (int i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i*verticalDistanceBetweenRays));

            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            /*
            if (i == 0 &&
                HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;*/

            deltaMovement.x = raycastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

            if (rayDistance < SkinWidth + .0001f)
            {
                break;
            }

        }

    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : Vector2.down;
        var rayOrigin = isGoingUp ? raycastTopLeft : raycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;
        for (int i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i*horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit) continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = myTransform.position.y - raycastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = raycastHit.collider.gameObject;
                }

            }

            deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
            }

            /*
            if (!isGoingUp && deltaMovement.y > .0001f)
                State.IsMovingUpSlope = true;
            */

            if (rayDistance < SkinWidth + .0001f)
                break;
        }
    }

    /*
    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
    
    }
    

    private void HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        return false;
    }
    */

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
    }

    //Pointing and pushing
    public void PointAndPush(float horizontalPoint, float verticalPoint, bool pushing)
    {
        if (pushing) Debug.Log("fisk");
        pointDirection = new Vector2(horizontalPoint, verticalPoint);
        var characterCenter = new Vector2(myTransform.position.x + boxCollider.offset.x, myTransform.position.y + boxCollider.offset.y);
        Debug.DrawLine(characterCenter, characterCenter + (pointDirection * 6), Color.cyan);
        
    }



}
