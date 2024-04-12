using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;

    private GameObject door;

    private Button b1;
    private Button b2;

    private bool test;

    // Start is called before the first frame update
    void Start()
    {
        b1 = button1.GetComponent<Button>();
        b2 = button2.GetComponent<Button>();

        door = GameObject.Find("Door");
    }

    // Update is called once per frame
    void Update()
    {
        if(b1.isTriggered && b2.isTriggered)
        {
            door.SetActive(false);
            test = true;
        }
        else if (test && !(b1.isTriggered && b2.isTriggered)) door.SetActive(true);
    }
}
