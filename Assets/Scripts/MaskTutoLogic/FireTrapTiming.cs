using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapTiming : MonoBehaviour
{
    public Animator animator;
    public float waitingTimeFlamethrower;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitOnce());
    }

    public IEnumerator WaitOnce()
    {
        yield return new WaitForSeconds(waitingTimeFlamethrower);

        StartCoroutine(WaitLightUp());
    }

    public IEnumerator WaitLightUp()
    {   
        // activate
        animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(2);

        // de-activate
        animator.SetBool("isFiring", false);
        yield return new WaitForSeconds(0.80f);

        StartCoroutine(WaitLightUp());
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && animator.GetBool("isFiring") == true)
        {
            collision.gameObject.transform.position = collision.gameObject.GetComponent<Movement>().respawnPoint;
        }
    }
}
