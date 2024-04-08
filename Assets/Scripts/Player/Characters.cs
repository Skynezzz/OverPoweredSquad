using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Characters : MonoBehaviour
{
    PlayerPowersData playerPowersData;

    public Animator animator;

    public RuntimeAnimatorController Frog;
    public RuntimeAnimatorController Bear;
    public RuntimeAnimatorController Diver;
    public RuntimeAnimatorController Mask;
    
    FrogPower frogPower;
    BearPower bearPower;
    DiverPower diverPower;
    MaskPower maskPower;

    Powers currentPower;

    private void Start()
    {
        playerPowersData = GetComponent<PlayerPowersData>();

        frogPower = new(playerPowersData);
        bearPower = new(playerPowersData);
        diverPower = new(playerPowersData);
        maskPower = new(playerPowersData);

        currentPower = frogPower;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPower.Power();
        }

        // SWITCH
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.runtimeAnimatorController = Frog;
            currentPower = frogPower;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            animator.runtimeAnimatorController = Bear;
            currentPower = bearPower;

        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            animator.runtimeAnimatorController = Diver;
            currentPower = diverPower;

        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            animator.runtimeAnimatorController = Mask;
            currentPower = maskPower;
        }
    }
}

class Powers : MonoBehaviour
{
    protected PlayerPowersData PPD;

    public Powers(PlayerPowersData playerPowersData)
    {
        PPD = playerPowersData;
    }
    public virtual void Power() { }
}

class FrogPower : Powers
{
    public FrogPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void Power()
    {
        if (PPD.movement.doubleJump)
        {

            PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x, PPD.movement.jumpStrengh * PPD.movement.jumpMultiplicater);

            PPD.movement.doubleJump = false;

            if (PPD.movement.isGrounded && !PPD.movement.isWalled) PPD.movement.doubleJump = true;
        } 
    }
}

class MaskPower : Powers
{
    public MaskPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }
    
    public override void Power()
    {
        if (PPD.spriteRenderer.flipX == true)
        {
            GameObject bullet = Instantiate(PPD.bulletSpawner.bulletPrefab, PPD.bulletSpawner.bulletSpawnPointLeft.position, PPD.bulletSpawner.bulletSpawnPointLeft.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = -PPD.bulletSpawner.bulletSpawnPointLeft.right * PPD.bulletSpawner.bulletSpeed;
        }
        else
        {
            GameObject bullet = Instantiate(PPD.bulletSpawner.bulletPrefab, PPD.bulletSpawner.bulletSpawnPointRight.position, PPD.bulletSpawner.bulletSpawnPointRight.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = PPD.bulletSpawner.bulletSpawnPointRight.right * PPD.bulletSpawner.bulletSpeed;
        }
    }
}

class DiverPower : Powers
{
    public DiverPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void Power()
    {
        // utilise de l'eau ou jsp
    }
}

class BearPower : Powers
{
    public BearPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void Power()
    {
        // casser si objet cassable en face de lui 
    }
}

