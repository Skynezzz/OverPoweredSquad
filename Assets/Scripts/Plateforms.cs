using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateforms : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFeet")
        {
            if (Input.GetKey(KeyCode.S))
            {
                gameObject.GetComponent<CompositeCollider2D>().isTrigger = true;
            }
            else
            {
                gameObject.GetComponent<CompositeCollider2D>().isTrigger = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        gameObject.GetComponent<CompositeCollider2D>().isTrigger = true;
    }
}
