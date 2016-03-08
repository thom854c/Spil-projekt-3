using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float cameraStartX, cameraEndX, cameraLengthFromplayerX, cameraStartY, cameraEndY, cameraLengthFromplayerYUp, cameraLengthFromplayerYDown;


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
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, GameObject.Find("Player").transform.position.x - cameraLengthFromplayerX, GameObject.Find("Player").transform.position.x + cameraLengthFromplayerX), 
            Mathf.Clamp(transform.position.y, GameObject.Find("Player").transform.position.y - cameraLengthFromplayerYDown, GameObject.Find("Player").transform.position.y + cameraLengthFromplayerYUp), 
            transform.position.z);
        //transform.position = new Vector3(GameObject.Find("Player").transform.position.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraStartX, cameraEndX), 
            Mathf.Clamp(transform.position.y, cameraStartY, cameraEndY), 
            transform.position.z);
    }
}
