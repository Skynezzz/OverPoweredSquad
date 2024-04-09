using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        playerMovement.booleens[selectedTrigger] = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.booleens[selectedTrigger] = false;
    }
}