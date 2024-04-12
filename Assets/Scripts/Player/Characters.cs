using UnityEngine;

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
        currentPower.Update();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPower.Power();
        }

        // SWITCH CHARACTERS
        if (Input.GetKey(KeyCode.Alpha1))
        {
            currentPower.OnExit();
            animator.runtimeAnimatorController = Frog;
            currentPower = frogPower;
            currentPower.OnEnter();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            currentPower.OnExit();
            animator.runtimeAnimatorController = Bear;
            currentPower = bearPower;
            currentPower.OnEnter();

        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            currentPower.OnExit();
            animator.runtimeAnimatorController = Diver;
            currentPower = diverPower;
            currentPower.OnEnter();

        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            currentPower.OnExit();
            animator.runtimeAnimatorController = Mask;
            currentPower = maskPower;
            currentPower.OnEnter();

        }
        //
    }
}

class Powers : MonoBehaviour
{
    protected PlayerPowersData PPD;

    public Powers(PlayerPowersData playerPowersData)
    {
        PPD = playerPowersData;
    }
    public virtual void Update() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Power() { }
}

class FrogPower : Powers
{
    public FrogPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void OnEnter()
    {

    }

    public override void Power()
    {
        if (PPD.movement.booleens["isGrounded"])
        {

            PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x, PPD.movement.jumpStrengh * PPD.jumpMultiplicater);

            PPD.movement.booleens["doubleJump"] = false;

            if (PPD.movement.booleens["isGrounded"] && !PPD.movement.booleens["isWalled"]) PPD.movement.booleens["doubleJump"] = true;
        } 
    }

    public override void OnExit()
    {

    }
}

class MaskPower : Powers
{
    public MaskPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    float oldMoveStrengh;
    float oldMaxRunSpeed;

    public override void OnEnter()
    {
        oldMoveStrengh = PPD.movement.moveStrengh;
        PPD.movement.moveStrengh = PPD.movement.moveStrengh * PPD.moveStreghMultiplicator;
        oldMaxRunSpeed = PPD.movement.moveStrengh;
        PPD.movement.maxRunSpeed = PPD.movement.maxRunSpeed * PPD.maxRunSpeedMultiplicator;
    }
    public override void Power()
    {
        if (PPD.spriteRenderer.flipX == true)
        {
            GameObject bullet = Instantiate(PPD.bulletSpawner.bulletPrefab, PPD.bulletSpawner.bulletSpawnPointLeft.position, PPD.bulletSpawner.bulletSpawnPointLeft.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = -PPD.bulletSpawner.bulletSpawnPointLeft.right * PPD.bulletSpawner.bulletSpeed;
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GameObject bullet = Instantiate(PPD.bulletSpawner.bulletPrefab, PPD.bulletSpawner.bulletSpawnPointRight.position, PPD.bulletSpawner.bulletSpawnPointRight.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = PPD.bulletSpawner.bulletSpawnPointRight.right * PPD.bulletSpawner.bulletSpeed;
        }
    }
    public override void OnExit()
    {
        PPD.movement.moveStrengh = oldMoveStrengh;
        PPD.movement.maxRunSpeed = oldMaxRunSpeed;
    }
}

class DiverPower : Powers
{
    float remaningCdTime;
    bool hthDash;
    float oldWaterMoveStrengh;
    public DiverPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void Update()
    {
        hthDash = PPD.movement.booleens["isGrounded"] || PPD.dash || hthDash;
        if (remaningCdTime <= 0)
        {
            PPD.dash = PPD.movement.booleens["isGrounded"] || hthDash;
            if (PPD.movement.booleens["isGrounded"]) hthDash = true;
        }
        else
        {
            remaningCdTime -= Time.deltaTime;
        }
    }

    public override void OnEnter()
    {
        PPD.respirationTime = 99999;

        oldWaterMoveStrengh = PPD.movement.waterMoveStrengh;
        PPD.movement.waterMoveStrengh = PPD.movement.waterMoveStrengh * PPD.waterMoveStrenghMultiplicator;
    }

    public override void Power()
    {
        if (PPD.dash)
        {
            switch((int)PPD.movement.movingDirection)
            {
                case (int)Movement.Direction.None:
                    return;
                case (int)Movement.Direction.Left:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x - PPD.dashStrenght, PPD.rigidbody2D.velocity.y);
                    break;
                case (int)Movement.Direction.Right:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x + PPD.dashStrenght, PPD.rigidbody2D.velocity.y);
                    break;
                case (int)Movement.Direction.Down:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x, PPD.rigidbody2D.velocity.y - PPD.dashStrenght);
                    break;
                case (int)Movement.Direction.Up:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x, PPD.rigidbody2D.velocity.y + PPD.dashStrenght);
                    break;
                case (int)Movement.Direction.Up + (int)Movement.Direction.Left:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x - PPD.dashStrenght / 2, PPD.rigidbody2D.velocity.y + PPD.dashStrenght / 2);
                    break;
                case (int)Movement.Direction.Down + (int)Movement.Direction.Left:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x - PPD.dashStrenght / 2, PPD.rigidbody2D.velocity.y - PPD.dashStrenght / 2);
                    break;
                case (int)Movement.Direction.Up + (int)Movement.Direction.Right:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x + PPD.dashStrenght / 2, PPD.rigidbody2D.velocity.y + PPD.dashStrenght / 2);
                    break;
                case (int)Movement.Direction.Down + (int)Movement.Direction.Right:
                    PPD.rigidbody2D.velocity = new(PPD.rigidbody2D.velocity.x + PPD.dashStrenght / 2, PPD.rigidbody2D.velocity.y - PPD.dashStrenght / 2);
                    break;
            }

            remaningCdTime = PPD.dashCd;
            PPD.dash = false;
            if (!PPD.movement.booleens["isGrounded"]) hthDash = false;
        }
    }

    public override void OnExit()
    {
        PPD.respirationTime = 6;

        PPD.movement.waterMoveStrengh = oldWaterMoveStrengh;
    }
}

class BearPower : Powers
{
    float oldMass;
    public override void OnEnter()
    {
        oldMass = PPD.rigidbody2D.mass;
        PPD.rigidbody2D.mass = PPD.massMultiplicator;
    }
    public BearPower(PlayerPowersData playerPowersData) : base(playerPowersData) { }

    public override void Power()
    {
        print("oui");
        BulletSpawner bulletSpawner = PPD.gameObject.GetComponent<BulletSpawner>();
        BrickBreaker leftPoint = bulletSpawner.bulletSpawnPointLeft.gameObject.GetComponent<BrickBreaker>();
        BrickBreaker rightPoint = bulletSpawner.bulletSpawnPointRight.gameObject.GetComponent<BrickBreaker>();

        if (PPD.spriteRenderer.flipX == true)
        {
            print("flip");
            if (leftPoint.isTriggered == true)
            {
                Destroy(leftPoint.brick);
            }
        }
        else
        {
            print("hein?");
            if (rightPoint.isTriggered == true)
            {
                print(rightPoint);
                Destroy(rightPoint.brick);
            }
        }
    }
    public override void OnExit()
    {
        PPD.rigidbody2D.mass = oldMass;
    }
}

