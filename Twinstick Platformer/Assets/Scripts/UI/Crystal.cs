using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour 
{

    public GameObject CrystalParent;

    public bool IsActive;
	// Use this for initialization
	void Start () 
    {
        CrystalParent = GameObject.FindGameObjectWithTag("CrystalParent");

        IsActive = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsActive && CrystalParent.GetComponent<CrystalsScript>().CrystalNumber > CrystalParent.GetComponent<CrystalsScript>().Crystals.Count)
        {
            CrystalParent.GetComponent<CrystalsScript>().Crystals.Add(gameObject);

            IsActive = true;
            //Debug.Log("More Hearts");
        }
        else if (IsActive && CrystalParent.GetComponent<CrystalsScript>().CrystalNumber < CrystalParent.GetComponent<CrystalsScript>().Crystals.Count)
        {
            CrystalParent.GetComponent<CrystalsScript>().Crystals.Remove(gameObject);

            IsActive = false;

            gameObject.transform.position = new Vector3(1000, 1000, 0);
        }
        
	}
}
