using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator animator;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var gameLogic = GameObject.FindGameObjectWithTag("GameLogic");
            gameLogic.GetComponent<GameLogic>().score++;
            animator.SetBool("hasTriggered", true);
            Destroy(gameObject, 0.5f);
        }
    }
}
