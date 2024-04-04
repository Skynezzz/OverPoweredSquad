using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Animator animator;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("Checking", true);
    }

}
