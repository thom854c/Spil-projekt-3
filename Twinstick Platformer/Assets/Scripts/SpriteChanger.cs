using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour 
{
    public Sprite checkpointFaded;
    public Sprite checkpointActive;

    private SpriteRenderer spriteRenderer;

    public AudioSource lyd;

    private GameObject[] checkPoints;

	// Use this for initialization
	void Start () 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = checkpointFaded;
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ChangeSprite();
            Debug.Log("gotcha");
            
        }
    }

    void ChangeSprite()
    {
        if (spriteRenderer.sprite == checkpointFaded)
        {
            spriteRenderer.sprite = checkpointActive;
            DisableOtherCheckPoints();
            lyd.Play();

        }

    }
	// Update is called once per frame
	void Update () 
    {

	
	}

    /// <summary>
    /// Find alle checkpoints og fade deres sprites (undtagen denne instance);
    /// </summary>
    void DisableOtherCheckPoints()
    {
        string _curCheckPointName = transform.name;
        //Debug.Log(_curCheckPointName);
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject cp in checkPoints){
            if (cp.transform.name != _curCheckPointName)
            {
                cp.GetComponent<SpriteRenderer>().sprite = checkpointFaded;
               // Debug.Log(cp.transform.name);
            }
        }
    }
}
