using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

    //public Vector2 HeartDirection;

	// Use this for initialization
	void Start () 
    {
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < Hearts.Count; i++)
        {

            while (Hearts[i] == null)
            {
                Hearts.RemoveAt(i);
            }

            var angle = i * AngleIncrease + StartAngle;
            Hearts[i].transform.position = new Vector3(HeartsCircleRadius * Mathf.Cos(angle) + transform.position.x, HeartsCircleRadius * Mathf.Sin(angle) + transform.position.y, 0);
        }
      
	}
}
