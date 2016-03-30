using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour
{

    public int MissileSpeed;

	void Update () 
    {
	    transform.Translate(new Vector3(0,MissileSpeed,0)*Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                StaticVariables.PlayerHealth--;
                if (StaticVariables.PlayerHealth >0)
                {
                    GameObject.Find("Player").GetComponent<Player>().HitSound.Play();
                }

                Destroy(gameObject);
                break;
            case "Enemy":
                Destroy(gameObject);
                break;
            case "Boss":
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
