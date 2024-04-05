using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player_Collision : MonoBehaviour
{
    public GameObject player;
    public string selectedTrigger;
    Player sPlayer;

    public void Start()
    {
        sPlayer = player.GetComponent<Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            switch (selectedTrigger)
            {
                case "isGrounded":
                    sPlayer.isGrounded = true;
                    break;
                    
                case "isWalledLeft":
                    sPlayer.isWalledLeft = true;
                    break;
                    
                case "isWalledRight":
                    sPlayer.isWalledRight = true;
                    break;

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            switch (selectedTrigger)
            {
                case "isGrounded":
                    sPlayer.isGrounded = false;
                    break;

                case "isWalledLeft":
                    sPlayer.isWalledLeft = false;
                    break;

                case "isWalledRight":
                    sPlayer.isWalledRight = false;
                    break;

            }
        }
    }
}