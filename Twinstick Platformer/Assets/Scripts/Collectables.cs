using UnityEngine;
using System.Collections;

public class Collectables : MonoBehaviour 
{

    void OnTriggerEnter2D(Collider2D other)

    {
        if (other.tag == "Player" && gameObject.tag == "Collectable")
        {
            StaticVariables.ObtainedCollectables ++;
            Debug.Log(StaticVariables.ObtainedCollectables);
            Destroy(gameObject);
        }

    }


}
