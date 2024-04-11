using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreaker : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject brick;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Brick") isTriggered = true;
        brick = collision.gameObject;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Brick") isTriggered = false;
        brick = null;
    }
}
