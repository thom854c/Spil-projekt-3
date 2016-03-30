using UnityEngine;
using System.Collections;

public class KeepThisObjectBetweenScenes : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	    if (GameObject.FindGameObjectWithTag("Music") != null)
	    {
            Object.DontDestroyOnLoad(gameObject);
	    }
	    else
	    {
	        Destroy(gameObject);
	    }
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKey(KeyCode.Escape))
	    {
	        Application.LoadLevel(0);
	    }


	}
}
