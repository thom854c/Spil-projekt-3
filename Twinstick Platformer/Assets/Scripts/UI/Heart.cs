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

        IsActive = false;
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
        else if (IsActive && HeartParent.GetComponent<HeartsScript>().HeartNumber < HeartParent.GetComponent<HeartsScript>().Hearts.Count)
        {
            HeartParent.GetComponent<HeartsScript>().Hearts.Remove(gameObject);

            IsActive = false;

            gameObject.transform.position = new Vector3(1000, 1000, 0);
        }
	}
}
