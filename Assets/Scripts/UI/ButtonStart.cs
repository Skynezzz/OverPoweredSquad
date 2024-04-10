using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStart : MonoBehaviour
{
    public void RunStart()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void RunLevels()
    {
        print("Show Levels!");
    }
    public void RunExit()
    {
        Application.Quit();
    }
}
