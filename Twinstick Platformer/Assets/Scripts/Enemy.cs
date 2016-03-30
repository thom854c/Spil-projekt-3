using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private CharacterController2D controller;
    [HideInInspector] public bool MoveLeft, MoveRight, JumpAfterPlayer,JumpRight, Attacking, ApproachingDeath, AttackFinished;
    public bool ChasePlayer;
    public int MoveSpeed ;
    public float PatrolLenght;
    private float delay, startPatrolTime, turnColdown, attackColdown, passiveSoundColdown;
    [HideInInspector]public float PatrolTime;
    private int patrolSpeed, direktion = 1, playerHealth;
    public AudioSource AttackSound,DieSound, PassiveSound;
    private bool wasPlaying;


    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        patrolSpeed = MoveSpeed;
        PatrolTime = PatrolLenght/MoveSpeed;
        startPatrolTime = PatrolLenght / MoveSpeed;
        playerHealth = StaticVariables.PlayerHealth;
    }

    public void Update()
    {
        if (ChasePlayer)
        {
            Chase();
        }
        Patrol();
        Attack();
        if (StaticVariables.EnemyHealth <= 0)
        {
            DieSound.Play();
            StaticVariables.EnemyHealth = 1000;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        if (DieSound.isPlaying)
        {
            wasPlaying = true;
        }
        else if (wasPlaying)
        {
            Destroy(gameObject);
        }
        passiveSoundColdown += Random.Range(0.2f, 1.2f) * Time.deltaTime;


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

        if (passiveSoundColdown> 20)
        {
            
            PassiveSound.Play();
            passiveSoundColdown = 0;
        }
    }

    void Attack()
    {
        
        attackColdown -= Time.deltaTime;
        if (Attacking && attackColdown < 0)
        {
                GetComponent<Animator>().SetBool("Attacking", true);
                attackColdown = 2;
            playerHealth = StaticVariables.PlayerHealth;

        }
        if(!Attacking)
        {
            GetComponent<Animator>().SetBool("Attacking", false);
        }
        if (AttackFinished && !DieSound.isPlaying)
        {
            StaticVariables.PlayerHealth = playerHealth-1;
            if (StaticVariables.PlayerHealth > 0)
            {
                GameObject.Find("Player").GetComponent<Player>().HitSound.Play();
            }
            GetComponent<Animator>().SetBool("Attacking", false);
        }
     
    }

}
