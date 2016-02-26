using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour 
{

    public GameObject HeartParent;

    public bool IsActive;
	// Use this for initialization
	void Start () 
    {
        HeartParent = GameObject.FindGameObjectWithTag("HeartParent");

        if (IsActive)
        {
            
            HeartParent.GetComponent<HeartsScript>().Hearts.Add(gameObject); 
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsActive && HeartParent.GetComponent<HeartsScript>().HeartNumber > HeartParent.GetComponent<HeartsScript>().Hearts.Count)
        {
            HeartParent.GetComponent<HeartsScript>().Hearts.Add(gameObject);

            IsActive = true;
            //Debug.Log("More Hearts");
        }
	}
}
