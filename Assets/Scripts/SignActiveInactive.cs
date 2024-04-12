using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SignBeara : MonoBehaviour
{
    private bool checking;

    private void Start()
    {
        checking = true;
    }

    void Update()
    {
        if (checking)
        {
            checking = false;
            StartCoroutine(SetActiveBrick());
        }
    }

    private IEnumerator SetActiveBrick()
    {
        TilemapRenderer tilemapRenderer = GetComponent<TilemapRenderer>();

        yield return new WaitForSeconds(1);

        tilemapRenderer.enabled = false;

        yield return new WaitForSeconds(2);

        tilemapRenderer.enabled = true;
        checking = true;
    }
}
