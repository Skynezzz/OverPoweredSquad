using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public Animator animator;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("Checking", true);
            StartCoroutine(EndRoutine());
        }
    }

    private IEnumerator EndRoutine()
    {
        // SET END ANIMATION
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(3);
    }
}
