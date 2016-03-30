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

        switch (other.tag)
        {
            case "Boss":
                StaticVariables.BossHealth --;
                Destroy(gameObject);
                break;
            case "Enemy":
                other.GetComponent<Enemy>().EnemyHealth --;
                Destroy(gameObject);
                break;
            case "Player":
                break;
            case "Missile":
                break;
            case "CheckPoint":
                break;
            case "PlayerDetector":
                break;
            case "Orb":
                break;
            case "FalseWall":
                break;
            case "Potion":
                break;
            default:
                Destroy(gameObject);
                break;
        }

    }
}
