using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour 
{

    //public GameObject Hearts;

    public List<GameObject> Hearts;
    
    //Used for calculating heart position 
    public float AngleIncrease;
    public float StartAngle;

    private float HeartsCircleRadius;

    //Number of Hearts the player has (value will probably come from another script)
    public int HeartNumber;

    //Maximum Health and Current Health, propably comes from another script
    public int MaxHP;
    public float CurHP;

    //Sprites for full, half and empty hearts
    public Sprite FullHeart;
    public Sprite HalfHeart;
    public Sprite EmptyHeart;

	// Use this for initialization
	void Start () 
    {
        HeartsCircleRadius = Screen.width / 16;

	}
	
	// Update is called once per frame
	void Update () 
    {
        HeartNumber = MaxHP / 2;

	    CurHP = StaticVariables.PlayerHealth;
	    MaxHP = StaticVariables.MaxHealth;

        //For loop for managing the hearts
        for (int i = 0; i < Hearts.Count; i++)
        {
            //Removes any empty space in the list
            while (Hearts[i] == null)
            {
                Hearts.RemoveAt(i);
            }

            //Places each heart in their correct position depending on position in list
            var angle = i * AngleIncrease + StartAngle;
            Hearts[i].transform.position = new Vector3(HeartsCircleRadius * Mathf.Cos(angle) + transform.position.x, HeartsCircleRadius * Mathf.Sin(angle) + transform.position.y, 0);

            //Checks which sprite to use, depending on current health
            if (i + 0.5f == CurHP / 2)
            {
                Hearts[i].GetComponent<Image>().sprite = HalfHeart;
            }
            else if (i + 1 > CurHP / 2)
            {
                Hearts[i].GetComponent<Image>().sprite = EmptyHeart;
            }
            else //if (i < CurHP / 2)
            {
                Hearts[i].GetComponent<Image>().sprite = FullHeart;
            }
        }
      
	}
}
