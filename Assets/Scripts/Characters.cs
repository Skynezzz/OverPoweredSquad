using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Characters : MonoBehaviour
{

    public Player player;

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
        frogPower = new(player);
        bearPower = new(player);
        diverPower = new(player);
        maskPower = new(player);

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
    protected Player player;

    public Powers(Player pPlayer)
    {
        player = pPlayer;
    }
    public virtual void Power() { }
}

class FrogPower : Powers
{
    public FrogPower(Player pPlayer) : base(pPlayer) { }

    public override void Power()
    {
        if (player.doubleJump)
        {
            
            player.rigidbody2D.velocity = new(player.rigidbody2D.velocity.x, player.jumpStrengh * player.jumpMultiplicater);

            player.doubleJump = false;

            if (player.isGrounded && !player.isWalled) player.doubleJump = true;
        } 
    }
}

class MaskPower : Powers
{
    public MaskPower(Player pPlayer) : base(pPlayer) { }
    
    public override void Power()
    {
        if (player.spriteRenderer.flipX == true)
        {
            var bullet = Instantiate(player.bulletPrefab, player.bulletSpawnPointLeft.position, player.bulletSpawnPointLeft.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = -player.bulletSpawnPointLeft.right * player.bulletSpeed;
        }
        else if (player.spriteRenderer.flipX == false)
        {
            var bullet = Instantiate(player.bulletPrefab, player.bulletSpawnPointRight.position, player.bulletSpawnPointRight.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = player.bulletSpawnPointRight.right * player.bulletSpeed;
        }
    }
}

class DiverPower : Powers
{
    public DiverPower(Player pPlayer) : base(pPlayer) { }

    public override void Power()
    {
        // utilise de l'eau ou jsp
    }
}

class BearPower : Powers
{
    public BearPower(Player pPlayer) : base(pPlayer) { }

    public override void Power()
    {
        // casser si objet cassable en face de lui 
    }
}

