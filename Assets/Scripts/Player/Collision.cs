using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player_Collision : MonoBehaviour
{
    public GameObject player;
    public string selectedTrigger;
    public bool test;
    Movement playerMovement;

    public void Start()
    {
        playerMovement = player.GetComponent<Movement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            test = true;
            switch (selectedTrigger)
            {
                case "isGrounded":
                    playerMovement.isGrounded = true;
                    break;
                    
                case "isWalledLeft":
                    playerMovement.isWalledLeft = true;
                    break;
                    
                case "isWalledRight":
                    playerMovement.isWalledRight = true;
                    break;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            test = false;
            playerMovement.isGrounded = false;
            playerMovement.isWalledLeft = false;
            playerMovement.isWalledRight = false;
        }
    }
}