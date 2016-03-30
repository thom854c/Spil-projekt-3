using UnityEngine;
using System.Collections;

public class MagicMissile : MonoBehaviour
{

    public int MissileSpeed;

    void Update()
    {
        transform.Translate(new Vector3(0, MissileSpeed, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Missile" || other.tag == "CheckPoint")
        {
            //do nothing
        }
        else if (other.tag == "Boss")
        {
            StaticVariables.BossHealth --;
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            StaticVariables.EnemyHealth --;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
