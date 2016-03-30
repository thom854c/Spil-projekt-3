using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrystalsScript : MonoBehaviour 
{
    

    //public GameObject Hearts;
    public List<GameObject> Crystals;
    
    //Player
    public GameObject Player;

    //Used for calculating heart position 
    public float CrystalsAngleIncrease;
    public float CrystalsStartAngle;

    private float CrystalsCircleRadius;

    //Number of Hearts the player has (value will probably come from another script)
    public int CrystalNumber;

    //Maximum mana and current mana, propably comes from another script
    public int Max_Mana;
    public int Cur_Mana;

    //Sprites for empty and full mana crystal
    public Sprite EmptyCrystal;
    public Sprite FullCrystal;

	// Use this for initialization
	void Start () 
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player.name == "Player (clone)")
        {
            Destroy(Player);
            Player = GameObject.FindGameObjectWithTag("Player");            
        }
        CrystalsCircleRadius = Screen.width/14;
        //Max_Mana = GetComponent<Player>().MaxMana;
        //Cur_Mana = GetComponent<Player>().curMana;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Max_Mana = Player.GetComponent<Player>().MaxMana;
        Cur_Mana = Player.GetComponent<Player>().curMana;
        CrystalNumber = Max_Mana;

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
            if (i + 1 > Cur_Mana)
            {
                Crystals[i].GetComponent<Image>().sprite = EmptyCrystal;
            }
            else if (i < Cur_Mana)
            {
                Crystals[i].GetComponent<Image>().sprite = FullCrystal; 
            }

        }
      
	}
}
