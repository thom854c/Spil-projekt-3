using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CrystalsScript : MonoBehaviour 
{

    //public GameObject Hearts;

    public List<GameObject> Crystals;
    
    //Used for calculating heart position 
    public float CrystalsCircleRadius;
    public float CrystalsAngleIncrease;
    public float CrystalsStartAngle;

    //Number of Hearts the player has (value will probably come from another script)
    public int CrystalNumber;

    //public Vector2 HeartDirection;

	// Use this for initialization
	void Start () 
    {
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < Crystals.Count; i++)
        {

            while (Crystals[i] == null)
            {
                Crystals.RemoveAt(i);
            }

            var angle = i * CrystalsAngleIncrease + CrystalsStartAngle;
            Crystals[i].transform.position = new Vector3(CrystalsCircleRadius * Mathf.Cos(angle) + transform.position.x, CrystalsCircleRadius * Mathf.Sin(angle) + transform.position.y, 0);
        }
      
	}
}
