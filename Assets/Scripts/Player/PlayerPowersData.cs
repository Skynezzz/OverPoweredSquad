using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowersData : MonoBehaviour
{
    [SerializeField] private GameObject goPlayer;

    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Movement movement;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public BulletSpawner bulletSpawner;

    //float
    public float jumpMultiplicater;
    public float respirationTime;
    public float dashStrenght;
    public float dashCd;
    //bool
    [HideInInspector] public bool dash;

    private void Start()
    {
        goPlayer = gameObject;

        rigidbody2D = goPlayer.GetComponent<Rigidbody2D>();
        movement = goPlayer.GetComponent<Movement>();
        spriteRenderer = goPlayer.GetComponent<SpriteRenderer>();
        bulletSpawner = goPlayer.GetComponent<BulletSpawner>();
    }

    public GameObject CreateEntity(GameObject objectPrefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(objectPrefab, position, rotation);
    }
}