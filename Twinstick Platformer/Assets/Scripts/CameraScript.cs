using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    FollowPlayer();
	}

    void FollowPlayer()
    {
        transform.position = new Vector3(GameObject.Find("Player").transform.position.x, transform.position.y, transform.position.z);

    }
}
