using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private bool isFacingRight, hasFired;
    private CharacterController2D controller;

    
    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public float DeaccelerationOnGround = 0.97f, DeaccelerationInAir = 0.99f, beforeManaRecharceTime, manaRecharceTime;
    public int curMana, MaxMana, pushManaCost;
    public bool canPoint = true, manaUsedThisFrame = false;
    public Animator anim;
    public GameObject MagicMissile;
    public AudioSource MissileSound, HitSound, LowHPSound, DeadSound, JumpSound, WalkSound;
    public float manaTimer, manaRecharceTimer;
    private string controllerString;
    private bool OnGroundLastFrame;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
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
        HandleAnimation();
        FireMissile();
        HealthSounds();
        LandSound();

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

    private void HandleAnimation()
    {
        if ((controller.Velocity.x > 0f && Input.GetAxis("Horizontal") > 0f) || (controller.Velocity.x < 0f && Input.GetAxis("Horizontal") < 0f))
        {
            anim.SetBool("horizontalMovement", true);
        }
        else
        {
            anim.SetBool("horizontalMovement", false);
        }

       anim.SetBool("onGround", controller.State.IsGrounded);
    }

    private void FireMissile()
    {

        if (controllerString == "Microsoft ")
        {

            if (Input.GetAxisRaw(controllerString + "R2") < 0 && controller.PointDebugActive.gameObject.active &&
                !hasFired && curMana > 0)
            {
                Instantiate(MagicMissile, transform.position,
                    Quaternion.Euler(0, 0,
                        Mathf.Rad2Deg*(Mathf.Atan2(controller.pointVector.y, controller.pointVector.x)) - 90));
                MissileSound.Play();
                hasFired = true;
                curMana--;
                manaUsedThisFrame = true;
            }
            else if (Input.GetAxisRaw(controllerString + "R2") == 0)
            {
                hasFired = false;
            }
        }
        else
        {

            if (Input.GetButtonDown(controllerString + "R2") && controller.PointDebugActive.gameObject.active &&
                !hasFired && curMana > 0)
            {
                Instantiate(MagicMissile, transform.position,
                    Quaternion.Euler(0, 0,
                        Mathf.Rad2Deg*(Mathf.Atan2(controller.pointVector.y, controller.pointVector.x)) - 90));
                MissileSound.Play();
                hasFired = true;
                curMana --;
                manaUsedThisFrame = true;
            }
            else if (!Input.GetButtonDown(controllerString + "R2"))
            {
                hasFired = false;
            }

        }

    }

    private void HealthSounds()
    {
        if (StaticVariables.PlayerHealth<=2)
        {
            LowHPSound.enabled = true;
        }
        else
        {
            LowHPSound.enabled = false;
        }

    }

    private void LandSound()
    {

        if (controller.State.IsGrounded && !OnGroundLastFrame)
        {
            JumpSound.Play();
        }
        OnGroundLastFrame = controller.State.IsGrounded;
    }


    private void WalkSoundPlay()
    {
        WalkSound.Play();
    }
}
