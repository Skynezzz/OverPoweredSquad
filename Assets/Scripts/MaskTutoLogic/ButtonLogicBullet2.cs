using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogicBullet2 : MonoBehaviour
{
    public GameObject button1;

    private ButtonBullet b1;

    // Start is called before the first frame update
    void Start()
    {
        b1 = button1.GetComponent<ButtonBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if(b1.isTriggeredBullet)
        {
            Destroy(GameObject.Find("DoorMask2"));
        }
    }
}
