using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform transform;
    public Animator animator;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("Checking", true);
            collision.GetComponent<Movement>().setRespawnPoint(transform);
        }
    }

}
