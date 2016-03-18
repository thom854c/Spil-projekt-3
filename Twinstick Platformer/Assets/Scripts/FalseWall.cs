using UnityEngine;
using System.Collections;

public class FalseWall : MonoBehaviour
{

    public float duration = 1.0f;


    public GameObject HideWall;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HideWall.SetActive(false);

        }

    }


   public void Update()
    {
       if (HideWall.gameObject.active == false)
       {
        //GetComponent<SpriteRenderer>().color = new Color(0, 0, 0,  Mathf.SmoothStep(maximum, minimum, t));
        iTween.FadeTo(gameObject,0f,duration);
       }

       

    }


}
