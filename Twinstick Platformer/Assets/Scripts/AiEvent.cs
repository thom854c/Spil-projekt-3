using UnityEngine;
using System.Collections;

public class AiEvent : MonoBehaviour
{
    public bool JumpRight;
    public bool ApproachingDeath;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (ApproachingDeath)
            {
                enemy.ApproachingDeath = true;
                enemy.PatrolTime = 0;
            }
            else
            {
                enemy.JumpRight = JumpRight;
                enemy.Jump();
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ApproachingDeath = false;
        }

    }
}
