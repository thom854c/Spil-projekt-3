using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour 
{
	
    public GameObject savedCheckpoint;
    public float lowerDeath = 4;
    public bool playerMortal;
    public float deathDelay;
    float deathTimer;
// Use this for initialization
	void Start () 
    {
        //Sætter startvectoren til checkpointed

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (transform.position.y < lowerDeath)
        {
            Die();
        }
        if (!playerMortal)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer > deathDelay)
            {
                playerMortal = true;
                deathTimer = 0;
            }

        }
	
	}

    void Die()
    {
        if (playerMortal)
        {
            transform.position = savedCheckpoint.transform.position;
            playerMortal = false;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) //Tjekker for at se om gameobjekted kolliderer med et bestemt andet objekt
    {
        // Tjekker hvilket type objekt der kollideres med
        if (other.tag == "CheckPoint")
        {
            //Ændre positionen på den nyeste vectorposition til at være den samme som gameobjektet
            savedCheckpoint = other.gameObject;
           // GameVariables.newestCheckPoint = transform.position;
            Debug.Log("Saved");


        }

        if (other.tag == "Trap")
        {
            //Ændre positionen på det kolliderende objekt til den sidste registrede checkpoint possition
            Die();
            Debug.Log("Trigger");
        }
    }
}
