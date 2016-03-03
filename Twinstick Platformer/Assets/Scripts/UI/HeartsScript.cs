using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour 
{

    //public GameObject Hearts;

    public List<GameObject> Hearts;
    
    //Used for calculating heart position 
    public float HeartsCircleRadius;
    public float AngleIncrease;
    public float StartAngle;

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
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        HeartNumber = MaxHP / 2;

        for (int i = 0; i < Hearts.Count; i++)
        {
            while (Hearts[i] == null)
            {
                Hearts.RemoveAt(i);
            }

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
