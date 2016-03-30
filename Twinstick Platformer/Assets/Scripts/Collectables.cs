using UnityEngine;
using System.Collections;

public class Collectables : MonoBehaviour 
{

    void OnTriggerEnter2D(Collider2D other)

    {
        if (other.tag == "Player" && gameObject.tag == "Collectable")
        {
            StaticVariables.ObtainedCollectables ++;
            Destroy(gameObject);
        }
        else if (other.tag == "Player" && gameObject.tag == "Potion")
        {
            StaticVariables.PlayerHealth += 4;
            Destroy(gameObject);
        }

    }


}
