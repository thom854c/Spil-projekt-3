using UnityEngine;
using System.Collections;

public class HitsObjectChecker : MonoBehaviour
{
    public bool CollisionStay;
    public bool TurnedOff;
    public bool CollisionIsChecked = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platforms") && !TurnedOff)
        {
                CollisionStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            CollisionStay = false;
        }
    }

    public void TurnOn()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        TurnedOff = false;
    }

    public void TurnOff()
    {
        CollisionStay = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        TurnedOff = true;
    }

}
