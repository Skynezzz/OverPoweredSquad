using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [HideInInspector] public GameLogic gameLogic;
    [HideInInspector] public TextMeshProUGUI tmp;
    private string coinsInLevel;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
        coinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length.ToString();
    }

    private void Update()
    {
        tmp.text = gameLogic.score.ToString() + " / " + coinsInLevel;
    }
}
