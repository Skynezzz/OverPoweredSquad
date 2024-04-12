using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector] public GameLogic gameLogic;
    [HideInInspector] public TextMeshProUGUI tmp;
    private float totalTime;
    private float remainingTime;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
        totalTime = gameLogic.levelTime;
        remainingTime = totalTime;
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            gameLogic.lose = true;
        }
        tmp.text = (int)remainingTime + " / " + (int)totalTime;
    }
}
