using UnityEngine;
using System.Collections;

public class DeadlyObject : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    StaticVariables.ActiveCheckpoint = GameObject.Find("Player").transform.position;
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
                
            case "Player":
                StaticVariables.PlayerHealth = 0;
                break;

            case "PlayerDetector":
                break;
            case "Orb":
                break;
            case "Enemy":
                StaticVariables.EnemyHealth = 0;
                break;
            default:
                Destroy(other.gameObject);
                break;
        }

    }
}
