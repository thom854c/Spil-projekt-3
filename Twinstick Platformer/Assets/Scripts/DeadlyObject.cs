using UnityEngine;
using System.Collections;

public class DeadlyObject : MonoBehaviour
{



	// Use this for initialization
	void Start ()
	{
	    StaticVariables.ActiveCheckpoint = GameObject.Find("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                other.transform.position = StaticVariables.ActiveCheckpoint;
                break;

            case "PlayerDetector":
                break;
            case "Orb":
                break;
            default:
                Destroy(other.gameObject);
                break;
        }

    }
}
