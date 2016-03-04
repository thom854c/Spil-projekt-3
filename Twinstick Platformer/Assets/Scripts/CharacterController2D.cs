using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{

    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get { return velocity; } }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return overrideParameters ?? DefaultParameters; } }
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

    private Vector2 characterRayVector;

    private float
        verticalDistanceBetweenRays,
        horizontalDistanceBetweenRays;

    // AddOn Pointing Variables
    private Vector2 pointVector;
    public GameObject PointDebugInactive, PointDebugActive;

    public float PointSensitivity = 0.3f, PointInactiveLenght = 3, PointActiveLength = 5, PushFrequency, PushMagnitude = 50;

    public bool PushCollides, PushInAir, Pushing;

    private bool pushingLastFrame, pushingLastLastFrame, pushingLastLastLastFrame;

    private float horizontalPoint, verticalPoint;
    private bool pushing, pushingPressed;


    // Staff Raycast and collision checking variables

    private const float StaffSkinWidth = .02f;
    private const int StaffTotalHorizontalRays = 4;
    private const int StaffTotalVerticalRays = 4;

    public StaffState2D StaffState { get; private set; }

    private Vector2 staffVelocity;
    private Vector2 amountPushed;
    private Vector3 staffPosition;
    private Vector3 staffLocalScale;
    private BoxCollider2D staffBoxCollider;

    private Vector3
        staffRaycastTopLeft,
        staffRaycastBottomRight,
        staffRaycastBottomLeft;

    private float
    staffVerticalDistanceBetweenRays,
    staffHorizontalDistanceBetweenRays;



    // Methods Begin
    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();
        myTransform = transform;
        localScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        var colliderHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);

        // Staff Awake
        StaffState = new StaffState2D();
        staffPosition = PointDebugActive.transform.position;
        staffLocalScale = PointDebugActive.transform.localScale;
        staffBoxCollider = PointDebugActive.GetComponent<BoxCollider2D>();

        var staffColliderWidth = staffBoxCollider.size.x * Mathf.Abs(staffLocalScale.x) - (2 * StaffSkinWidth);
        staffHorizontalDistanceBetweenRays = staffColliderWidth / (StaffTotalVerticalRays - 1);

        var staffColliderHeight = staffBoxCollider.size.y * Mathf.Abs(staffLocalScale.y) - (2 * StaffSkinWidth);
        staffVerticalDistanceBetweenRays = staffColliderHeight / (StaffTotalHorizontalRays - 1);

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

    //Pointing and pushing

    public void PointAndPush(float horizontalPointing, float verticalPointing, bool pushingKey, bool pushingKeyDown)
    {
        horizontalPoint = horizontalPointing;
        verticalPoint = verticalPointing;
        pushing = pushingKey;
        pushingPressed = pushingKeyDown;
    }

    public void PointAndPushInLateUpdate()
    {
        var pointerActiveScript = PointDebugActive.GetComponent<HitsObjectChecker>();

        pointVector = new Vector2(horizontalPoint, verticalPoint);

        var characterCenter = new Vector2(myTransform.position.x + boxCollider.offset.x, myTransform.position.y + boxCollider.offset.y);
        Debug.DrawLine(characterCenter, characterCenter + (pointVector * 6), Color.cyan);

        Debug.DrawRay(pointerActiveScript.transform.position, pointVector);


        if (pointVector.magnitude > PointSensitivity)
        {
            var pointVectorAngle = 0f;

            if (pointVector.normalized.y > 0)
            {
                pointVectorAngle = (Mathf.Acos(pointVector.normalized.x)/(2*Mathf.PI))*8;
                
            }
            else
            {
                pointVectorAngle = ((Mathf.PI + (Mathf.PI - Mathf.Acos(pointVector.normalized.x))) / (2 * Mathf.PI)) * 8;
            }

            pointVectorAngle = Mathf.Round(pointVectorAngle);

            pointVector = new Vector2(Mathf.Cos(0.25f * Mathf.PI * pointVectorAngle), Mathf.Sin(0.25f * Mathf.PI * pointVectorAngle));

            PointDebugActive.transform.position = characterCenter + (pointVector.normalized * PointActiveLength);
            if (!pushing)
            {
                PointDebugInactive.SetActive(true);
                pointerActiveScript.TurnOff();
                PointDebugInactive.transform.position = characterCenter + (pointVector.normalized * PointInactiveLenght);
            }
            else
            {

                PointDebugInactive.SetActive(false);
                pointerActiveScript.TurnOn();
                //PointDebugActive.transform.position = characterCenter + (pointVector.normalized * PointActiveLength);
                staffPosition = characterCenter + (pointVector.normalized * PointActiveLength);
                if (pushingLastLastLastFrame)
                {
                    if (GetComponent<Player>() != null)
                    {
                        if (GetComponent<Player>().curMana > 0)
                        {
                            GetComponent<Player>().curMana -= GetComponent<Player>().pushManaCost;
                            GetComponent<Player>().manaUsedThisFrame = true;
                            if (PushInAir)
                            {
                                AddForce(new Vector2(-pointVector.normalized.x * PushMagnitude, -pointVector.normalized.y * (PushMagnitude * 0.5f)));
                            }
                            else if (pointerActiveScript.CollisionStay)
                            {

                                if (velocity.y <= 0)
                                {
                                    SetVerticalForce(-pointVector.normalized.y * PushMagnitude);
                                }
                                else
                                {
                                    AddForce(new Vector2(0, -pointVector.normalized.y * PushMagnitude));
                                }
                                AddForce(new Vector2(-pointVector.normalized.x * PushMagnitude, 0));


                            }

                        }
                    }
                    else
                    {
                        if (PushInAir)
                        {
                            AddForce(new Vector2(-pointVector.normalized.x * PushMagnitude, -pointVector.normalized.y * (PushMagnitude * 0.5f)));
                        }
                        else if (pointerActiveScript.CollisionStay)
                        {

                            if (velocity.y <= 0)
                            {
                                SetVerticalForce(-pointVector.normalized.y * PushMagnitude);
                            }
                            else
                            {
                                AddForce(new Vector2(0, -pointVector.normalized.y * PushMagnitude));
                            }
                            AddForce(new Vector2(-pointVector.normalized.x * PushMagnitude, 0));


                        }
                    }
                }

                //staffVelocity = PointDebugActive.transform.position - staffPosition;
            }
            pushingLastLastLastFrame = pushingLastFrame;
            pushingLastLastFrame = pushingLastFrame;
            pushingLastFrame = pushingPressed;
        }
        else
        {
            PointDebugInactive.SetActive(false);
            pointerActiveScript.TurnOff();
        }
    }


    public void LateUpdate()
    {
        PointAndPushInLateUpdate();
        jumpIn -= Time.deltaTime;
        velocity.y += Parameters.Gravity * Time.deltaTime;
        Move(Velocity * Time.deltaTime, (Velocity * Time.deltaTime) + staffVelocity);
    }

    private void Move(Vector2 deltaMovement, Vector2 staffDeltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();
        StaffState.Reset();

        if (HandleCollisions)
        {
            //HandlePlatforms();
            CalculateRayOrigins();
            StaffCalculateRayOrigins();
            //if (deltaMovement.y < 0 && wasGrounded) HandleVerticalSlope(ref deltaMovement);

            //StaffVelocityHorizontalMovement(ref deltaMovement);
            //StaffVelocityVerticalMovement(ref deltaMovement);


            if (Mathf.Abs(staffDeltaMovement.x) > .001f && Pushing)
            {
                //StaffMoveHorizontally(ref deltaMovement, ref staffDeltaMovement);
            }

            if (Mathf.Abs(deltaMovement.x) > .001f)
            {
                MoveHorizontally(ref deltaMovement);
            }

            //if (Pushing) StaffMoveVertically(ref deltaMovement, ref staffDeltaMovement);

            MoveVertically(ref deltaMovement);
        }

        myTransform.Translate(deltaMovement, Space.World);
        PointDebugActive.transform.position = staffPosition;

        //TODO: Moving platforms


        if (Time.deltaTime > 0)
        {
            velocity = deltaMovement / Time.deltaTime;
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
        var size = new Vector2(boxCollider.size.x * Mathf.Abs(localScale.x), boxCollider.size.y * Mathf.Abs(localScale.y)) / 2;
        var center = new Vector2(boxCollider.offset.x * localScale.x, boxCollider.offset.y * localScale.y);

        raycastTopLeft = myTransform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        raycastBottomRight = myTransform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        raycastBottomLeft = myTransform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);


    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x + amountPushed.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? raycastBottomRight : raycastBottomLeft;

        for (int i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalDistanceBetweenRays));

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
            var rayVector = new Vector2(rayOrigin.x + (i * horizontalDistanceBetweenRays), rayOrigin.y);
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

    // Staff Collision
    private void StaffCalculateRayOrigins()
    {
        var staffSize = new Vector2(staffBoxCollider.size.x * Mathf.Abs(staffLocalScale.x), staffBoxCollider.size.y * Mathf.Abs(staffLocalScale.y)) / 2;
        var staffCenter = new Vector2(staffBoxCollider.offset.x * staffLocalScale.x, staffBoxCollider.offset.y * staffLocalScale.y);

        staffRaycastTopLeft = staffPosition + new Vector3(staffCenter.x - staffSize.x + StaffSkinWidth, staffCenter.y + staffSize.y - StaffSkinWidth);
        staffRaycastBottomRight = staffPosition + new Vector3(staffCenter.x + staffSize.x - StaffSkinWidth, staffCenter.y - staffSize.y + StaffSkinWidth);
        staffRaycastBottomLeft = staffPosition + new Vector3(staffCenter.x - staffSize.x + StaffSkinWidth, staffCenter.y - staffSize.y + StaffSkinWidth);
    }

    private void StaffMoveHorizontally(ref Vector2 deltaMovement, ref Vector2 staffDeltaMovement)
    {
        var isGoingRight = deltaMovement.x - amountPushed.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + StaffSkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? staffRaycastBottomRight : staffRaycastBottomLeft;

        for (int i = 0; i < StaffTotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * staffVerticalDistanceBetweenRays));

            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.white);

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

            //deltaMovement.x -= staffVelocity.x;

            if (rayDistance < SkinWidth + .0001f)
            {
                break;
            }
        }
    }


    private void StaffMoveVertically(ref Vector2 deltaMovement, ref Vector2 staffDeltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + StaffSkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : Vector2.down;
        var rayOrigin = isGoingUp ? staffRaycastTopLeft : staffRaycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;
        for (int i = 0; i < StaffTotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * staffHorizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.white);

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

    private void StaffVelocityHorizontalMovement(ref Vector2 characterDeltaMovement)
    {
        var deltaMovement = staffVelocity;
        var isGoingRight = deltaMovement.x < 0;
        var rayDistance = deltaMovement.x + StaffSkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? staffRaycastBottomRight : staffRaycastBottomLeft;

        var pointerActiveScript = PointDebugActive.GetComponent<HitsObjectChecker>();

        for (int i = 0; i < StaffTotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * staffVerticalDistanceBetweenRays));

            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.blue);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            amountPushed.x = rayDistance - (raycastHit.point.x - rayVector.x);

            if (isGoingRight)
            {
                amountPushed.x -= StaffSkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                amountPushed.x += StaffSkinWidth;
                State.IsCollidingLeft = true;
            }
            if (amountPushed.x != 0)
            {
                transform.position = new Vector3(5, 20);
                //transform.Translate(-amountPushed.x, 0 ,0);
            }

            rayDistance = Mathf.Abs(deltaMovement.x);
        }
    }

    private void StaffVelocityVerticalMovement(ref Vector2 characterDeltaMovement)
    {
        var deltaMovement = staffVelocity;
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + StaffSkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : Vector2.down;
        var rayOrigin = isGoingUp ? staffRaycastTopLeft : staffRaycastBottomLeft;

        var pointerActiveScript = PointDebugActive.GetComponent<HitsObjectChecker>();

        rayOrigin.x += -amountPushed.x;
        var standingOnDistance = float.MaxValue;

        for (int i = 0; i < StaffTotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * staffHorizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.blue);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit) continue;

            /*
            if (i == 0 &&
                HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;*/
            amountPushed.y = rayDistance - (raycastHit.point.y - rayVector.y);

            if (isGoingUp)
            {
                amountPushed.y -= StaffSkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                amountPushed.y += StaffSkinWidth;
                State.IsCollidingBelow = true;
            }
            if (amountPushed.y != 0)
            {
                characterDeltaMovement.y += -amountPushed.y;
            }

            rayDistance = Mathf.Abs(deltaMovement.y);

        }


    }

}
