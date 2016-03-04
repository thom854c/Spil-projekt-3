using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour
{
    private Enemy enemy;

    public void Start()
    {
        enemy = GetComponentInParent<Enemy>();

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && other.transform.position.x > transform.parent.position.x + 6)
        {
            transform.parent.localScale = new Vector3(1, transform.parent.localScale.y, transform.parent.localScale.z);
            enemy.MoveRight = true;
            enemy.Attacking = false;

        }
        else if (other.tag == "Player" && other.transform.position.x < transform.position.x - 6)
        {
            transform.parent.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            enemy.MoveLeft = true;
            enemy.Attacking = false;
        }
        else if (other.tag == "Player")
        {
            enemy.MoveRight = false;
            enemy.MoveLeft = false;
            enemy.Attacking = true;
        }

        if (other.tag == "Player" && other.transform.position.y+1 < transform.parent.position.y)
        {
            enemy.JumpAfterPlayer = false;
        }
        else if (other.tag == "Player")
        {
            enemy.JumpAfterPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.MoveRight = false;
            enemy.MoveLeft = false;
            enemy.Attacking = false;
        }
        
    }
}
