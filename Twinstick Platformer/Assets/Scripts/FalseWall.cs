using UnityEngine;
using System.Collections;

public class FalseWall : MonoBehaviour
{

    public float duration = 1.0f;

    public bool discovered;


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Missile")
        {
            discovered = true;
            GetComponentInChildren<FalseWall>().discovered = true;
            Destroy(other);
        }

    }


   public void Update()
    {

       if (discovered)
       {
           iTween.FadeTo(gameObject, 0f, duration);
           GetComponent<BoxCollider2D>().enabled = false;
     
           foreach (Transform child in transform)
           {
               if (child.gameObject.GetComponent<BoxCollider2D>() != null)
               {
                   child.gameObject.GetComponent<BoxCollider2D>().enabled = false;    
               }
           }
       }
       

    }


}
