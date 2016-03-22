using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour
{

    public int MissileSpeed;

	void Update () 
    {
	    transform.Translate(new Vector3(0,MissileSpeed,0)*Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StaticVariables.PlayerHealth --;
            Destroy(gameObject);
        }
        else if (other.tag == "Missile" || other.tag == "Boss")
        {
           //do nothing 
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
