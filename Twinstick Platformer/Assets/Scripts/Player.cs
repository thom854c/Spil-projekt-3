using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private bool isFacingRight;
    private CharacterController2D controller;


    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public float DeaccelerationOnGround = 0.97f, DeaccelerationInAir = 0.99f, beforeManaRecharceTime, manaRecharceTime;
    public int curMana, MaxMana, pushManaCost;
    public bool canPoint = true, manaUsedThisFrame = false;

    public float manaTimer, manaRecharceTimer;
    private string controllerString;

    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0;
        if (Input.GetJoystickNames().Length !=0)
        {
            if (Input.GetJoystickNames()[0] == "Wireless Controller")
            {
                controllerString = "Sony ";
            }
            else
            {
                controllerString = "Microsoft ";
            }   
        }
        else
        {
            controllerString = "Microsoft ";
        }   

    }

    public void Update()
    {
        HandleEnergy();
        HandleInput();

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;


        if (canPoint && !controller.Parameters.DisableAllMovement && !controller.Parameters.DisableControls)
        {
            controller.PointAndPush(Input.GetAxis(controllerString + "RS Horizontal"), Input.GetAxis(controllerString + "RS Vertical"), Input.GetButton(controllerString + "Push"), Input.GetButtonDown(controllerString + "Push"));
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

        if (controller.CanJump && Input.GetButtonDown(controllerString + "Jump"))
        {
            controller.Jump();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }

    public void HandleEnergy()
    {
        manaTimer += Time.deltaTime;
        if (manaUsedThisFrame)
        {
            manaTimer = 0;
        }

        if (beforeManaRecharceTime <= manaTimer)
        {
            manaRecharceTimer += Time.deltaTime;
            if (manaRecharceTimer >= manaRecharceTime)
            {
                if (curMana < MaxMana)
                {
                    manaRecharceTimer = 0f;
                    curMana++;    
                }
                
            }
        }
        else
        {
            manaRecharceTimer = 0;
        }

        manaUsedThisFrame = false;
    }
}
