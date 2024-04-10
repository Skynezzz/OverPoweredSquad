using UnityEngine;

public class Player_Collision : MonoBehaviour
{
    public GameObject player;
    public string selectedTrigger;
    Movement playerMovement;

    public void Start()
    {
        playerMovement = player.GetComponent<Movement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerMovement.asGround.Contains(collision.gameObject.tag)) playerMovement.booleens[selectedTrigger] = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerMovement.asGround.Contains(collision.gameObject.tag)) playerMovement.booleens[selectedTrigger] = false;
    }
}