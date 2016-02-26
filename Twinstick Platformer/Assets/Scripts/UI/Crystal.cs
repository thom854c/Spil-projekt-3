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

        if (IsActive)
        {
            
            CrystalParent.GetComponent<CrystalsScript>().Crystals.Add(gameObject); 
        }
        
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
	}
}
