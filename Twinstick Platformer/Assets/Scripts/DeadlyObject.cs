using UnityEngine;
using System.Collections;

public class DeadlyObject : MonoBehaviour
{

    float respawnTime;
    bool isDead;
    

	// Use this for initialization
	void Start ()
	{
	    StaticVariables.ActiveCheckpoint = GameObject.Find("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    respawnTime -= Time.deltaTime;
        if (gameObject.name == "Pit" && respawnTime < 0 && isDead)
        {

            StaticVariables.ResetBoss = true;
            StaticVariables.PlayerHealth = StaticVariables.MaxHealth;
            GameObject.Find("Player").transform.position = StaticVariables.ActiveCheckpoint;
            isDead = false;
        }
        else if (respawnTime < 0 && isDead)
        {
            StaticVariables.PlayerHealth = StaticVariables.MaxHealth;
            GameObject.Find("Player").transform.position = StaticVariables.ActiveCheckpoint;
            isDead = false;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
                
            case "Player":
                respawnTime = 5;
                isDead = true;
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
