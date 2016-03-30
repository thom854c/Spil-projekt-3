using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour 
{
	
    public GameObject savedCheckpoint;
    public float lowerDeath = 4;
    public bool playerMortal;
    public float deathDelay;
    float deathTimer, soundDelay = 2;
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

        if (StaticVariables.PlayerHealth == 0)
        {
            Die();
        }
	}

    void Die()
    {
        if (playerMortal)
        {
            GameObject.Find("Player").GetComponent<Player>().DeadSound.enabled = true;
            transform.position = savedCheckpoint.transform.position;
            StaticVariables.PlayerHealth = StaticVariables.MaxHealth;
            GetComponent<Player>().curMana = GetComponent<Player>().MaxMana;
            playerMortal = false;
        }
        if (GameObject.Find("Player").GetComponent<Player>().DeadSound.isPlaying)
        {
            soundDelay -= Time.deltaTime;
            Debug.Log(soundDelay);
        }
        else if (soundDelay < 2)
        {
            GameObject.Find("Player").GetComponent<Player>().DeadSound.enabled = false;
            soundDelay = 2;
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
