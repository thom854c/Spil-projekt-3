using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    //Maximum mana and current mana, propably comes from another script
    public int MaxMana;
    public int CurMana;

    //Sprites for empty and full mana crystal
    public Sprite EmptyCrystal;
    public Sprite FullCrystal;

	// Use this for initialization
	void Start () 
    {
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        CrystalNumber = MaxMana;

        //For loop for managing the mana crystals
        for (int i = 0; i < Crystals.Count; i++)
        {
            //Removes any empty space in the list
            while (Crystals[i] == null)
            {
                Crystals.RemoveAt(i);
            }

            //Places each crystal in their correct position depending on position in list 
            var angle = i * CrystalsAngleIncrease + CrystalsStartAngle;
            Crystals[i].transform.position = new Vector3(CrystalsCircleRadius * Mathf.Cos(angle) + transform.position.x, CrystalsCircleRadius * Mathf.Sin(angle) + transform.position.y, 0);

            //Checks which sprite to use, depending on current mana
            if (i + 1 > CurMana)
            {
                Crystals[i].GetComponent<Image>().sprite = EmptyCrystal;
            }
            else if (i < CurMana)
            {
                Crystals[i].GetComponent<Image>().sprite = FullCrystal; 
            }

        }
      
	}
}
