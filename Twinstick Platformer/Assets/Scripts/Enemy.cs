using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private CharacterController2D controller;
    [HideInInspector] public bool MoveLeft;
    [HideInInspector] public bool MoveRight;
    [HideInInspector] public bool JumpAfterPlayer;
    [HideInInspector] public bool JumpRight;
    [HideInInspector] public bool Attacking = false;
    [HideInInspector] public bool ApproachingDeath; 
    public bool ChasePlayer;
    public int Health = 3;
    public int MoveSpeed;
    public float PatrolLenght;
    private float delay;
    [HideInInspector]public float PatrolTime;
    private float startPatrolTime;
    private int patrolSpeed;
    private int direktion = 1;
    private float turnColdown;
    private float attackColdown;




    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        patrolSpeed = MoveSpeed;
        PatrolTime = PatrolLenght/MoveSpeed;
        startPatrolTime = PatrolLenght / MoveSpeed;
    }

    public void Update()
    {
        if (ChasePlayer)
        {
            Chase();
        }
        Patrol();
        Attack();
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void Jump()
    {

        if (JumpAfterPlayer && JumpRight && MoveRight || JumpAfterPlayer && !JumpRight && MoveLeft)
        {
            controller.Jump();
        }

    }

    void Chase()
    {
        if (MoveLeft && !ApproachingDeath)
        {
            controller.SetForce(new Vector2(-MoveSpeed, controller.Velocity.y));
            delay = 5;
        }
        else if (MoveRight && !ApproachingDeath)
        {
            controller.SetForce(new Vector2(MoveSpeed, controller.Velocity.y));
            delay = 5;
        }
        else if (!controller.State.IsGrounded)
        {
            //Do nothing
        }
        else
        {
            controller.SetForce(new Vector2(0, controller.Velocity.y));
        }    
    }

    void Patrol()
    {
        if (!ChasePlayer|| !MoveLeft && !MoveRight && controller.State.IsGrounded && !Attacking)
        {
            
            if (turnColdown > 0)
            {
                turnColdown -= 1 * Time.deltaTime;
            }

            if (delay > 0)
            {
                delay -= 1*Time.deltaTime;
            }
            else
            {
                controller.SetForce(new Vector2(patrolSpeed, controller.Velocity.y));
                transform.localScale = new Vector3(direktion, transform.localScale.y, transform.localScale.z);
                PatrolTime -= 1*Time.deltaTime;
            }



            if (PatrolTime <= 0 || controller.State.IsCollidingLeft && turnColdown <= 0 || controller.State.IsCollidingRight && turnColdown <= 0)
            {
                PatrolTime = startPatrolTime;
                patrolSpeed = -patrolSpeed;
                direktion = -direktion;
                turnColdown = 1f;
            }
        }
        else if(MoveLeft)
        {
            patrolSpeed = MoveSpeed;
            direktion = 1;
        }
        else if (MoveRight)
        {
            patrolSpeed = -MoveSpeed;
            direktion = -1;
        }
    }

    bool attacked = false;
    void Attack() 
    {

        if (Attacking && attackColdown <= 0)
        {
            attackColdown = 1;
            attacked = false;
        }


        if (attackColdown > 0)
        {
            attackColdown -= Time.deltaTime;
            if (attackColdown < 0.5f && !attacked && Attacking)
            {
                StaticVariables.PlayerHealth --;
                attacked = true;
            }
        }
    }

}
