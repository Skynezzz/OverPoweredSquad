using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCharacters : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public RuntimeAnimatorController Frog;
    public RuntimeAnimatorController Bear;
    public RuntimeAnimatorController Diver;
    public RuntimeAnimatorController Mask;

    // BULLETS //
    public Transform bulletSpawnPointRight;
    public Transform bulletSpawnPointLeft;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    // PRIVATE //
    private Dictionary<KeyCode, Tuple<>>

    private void Update()
    {

        // SWITCH //
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.runtimeAnimatorController = Frog;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            animator.runtimeAnimatorController = Bear;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            animator.runtimeAnimatorController = Diver;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            animator.runtimeAnimatorController = Mask;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Spell();
        }
    }
}


if (animator.runtimeAnimatorController == Mask)
{
    {
        if (spriteRenderer.flipX == true)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPointLeft.position, bulletSpawnPointLeft.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = -bulletSpawnPointLeft.right * bulletSpeed;
        }
        else if (spriteRenderer.flipX == false)
        { 
            var bullet = Instantiate(bulletPrefab, bulletSpawnPointRight.position, bulletSpawnPointRight.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPointRight.right * bulletSpeed;
        }
    }
}