using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour 
{
    public Sprite checkpointFaded;
    public Sprite checkpointActive;

    private SpriteRenderer spriteRenderer; 
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
        }

    }
	// Update is called once per frame
	void Update () 
    {

	
	}
}
