using System.Collections;
using UnityEngine;

public class ButtonTime : MonoBehaviour
{
    bool key = true;
    public GameObject door;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (key)
        {
            if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Bullet")
            {
                door.SetActive(false);
                key = false;

            }
        }

        if (key == false)
        {
            StartCoroutine(Timing());

        }
    }
    public IEnumerator Timing()
    {
        yield return new WaitForSeconds(20);

        door.SetActive(true);
        key = true;
    }
}

