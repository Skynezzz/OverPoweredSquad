using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBullet : MonoBehaviour
{
    public bool isTriggeredBullet = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet") isTriggeredBullet = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet") isTriggeredBullet = false;
    }

}
