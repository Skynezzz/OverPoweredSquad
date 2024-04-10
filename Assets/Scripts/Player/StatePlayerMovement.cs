using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StatePlayerMovement : MonoBehaviour
{
    public GameObject player;
    Movement playerMovement;

    public void Start()
    {
        playerMovement = player.GetComponent<Movement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerMovement.asMapItems.Contains(collision.gameObject.tag))
            playerMovement.booleens[collision.gameObject.tag] = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerMovement.asMapItems.Contains(collision.gameObject.tag))
            playerMovement.booleens[collision.gameObject.tag] = false;
    }
}