using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public int levelTime;
    [HideInInspector] public int score;
    [HideInInspector] public bool lose;

    private void Update()
    {
        if (lose)
        {
            StartCoroutine(BackToTheLobby());
        }
    }

    private IEnumerator BackToTheLobby()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(0);
    }
}