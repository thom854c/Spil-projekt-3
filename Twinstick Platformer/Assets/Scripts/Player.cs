using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private bool isFacingRight;
    private CharacterController2D controller;


    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public float DeaccelerationOnGround = 0.97f, DeaccelerationInAir = 0.99f;

    public bool canPoint = true;

    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0;

    }

    public void Update()
    {
        HandleInput();

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;


        if (canPoint && !controller.Parameters.DisableAllMovement && !controller.Parameters.DisableControls)
        {
            controller.PointAndPush(Input.GetAxis("RS Horizontal"), Input.GetAxis("RS Vertical"), Input.GetButton("Push"), Input.GetButtonDown("Push"));
        }


    }

    public void FixedUpdate()
    {
        if (controller.State.IsGrounded)
        {
            controller.SetHorizontalForce(controller.Velocity.x * DeaccelerationOnGround);
            if ((controller.Velocity.x >= MaxSpeed && Input.GetAxis("Horizontal") >= 0f) || (controller.Velocity.x <= -MaxSpeed && Input.GetAxis("Horizontal") <= -0f))
            {
                //do nothing   
            }
            else
            {
                controller.AddForce(new Vector2(Input.GetAxis("Horizontal") * SpeedAccelerationOnGround, 0f));
            }
        }
        else
        {
            controller.SetHorizontalForce(controller.Velocity.x * DeaccelerationInAir);
            if ((controller.Velocity.x >= MaxSpeed && Input.GetAxis("Horizontal") >= 0f) || (controller.Velocity.x <= -MaxSpeed && Input.GetAxis("Horizontal") <= -0f))
            {
                //do nothing   
            }
            else
            {
                controller.AddForce(new Vector2(Input.GetAxis("Horizontal") * SpeedAccelerationInAir, 0f));
            }
        }

    }

    private void HandleInput()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && isFacingRight)
        {
            Flip();
        }

        if (controller.CanJump && Input.GetButtonDown("Jump"))
        {
            controller.Jump();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }
}
