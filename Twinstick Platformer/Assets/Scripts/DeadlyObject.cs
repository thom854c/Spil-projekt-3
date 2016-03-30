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
                other.GetComponent<Enemy>().DieSound.Play();
                other.GetComponent<SpriteRenderer>().enabled = false;
                break;
            default:
                Destroy(other.gameObject);
                break;
        }

    }
}
