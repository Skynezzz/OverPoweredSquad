using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public enum Direction
    {
        None = 0,
        Up = 3,
        Down = -3,
        Left = -1,
        Right = 1
    }


    // FIELDS //

    // PUBLIC UNITY //
    //Objects
    public Transform respawnPoint;
    //Float
    public float moveStrengh;
    public float waterMoveStrengh;
    public float slowStrengh;
    public float maxRunSpeed;
    public float jumpStrengh;
    public float wallJumpStrengh;
    public float airControl;
    public float groundFriction;
    public float airFriction;
    public float waterFriction;
    public float wallRideDropSpeed;
    public float leaderRideSpeed;
    //Bool
    public Dictionary<string, bool> booleens;
    public List<string> asGround;
    public List<string> asMapItems;

    // HIDE UNITY //
    [HideInInspector] public Direction movingSide;
    [HideInInspector] public Direction movingDirection;

    // PRIVATE //

    // COMPONENTS //
    private Transform transform;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // FIELDS //


    // FUNCTIONS //

    private void Start()
    {
        respawnPoint = transform;

        booleens = new Dictionary<string, bool>
        {
            { "doubleJump", false },
            { "isGrounded", false },
            { "isWalled", false },
            { "isWalledLeft", false },
            { "isWalledRight", false },
            { "Water", false },
            { "Leader", false }
        };
        
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync(0);
        }
        if (transform.position.y < -100)
        {
            respawn();
        }
        if (booleens["Water"])
        {
            SetBool();

            MoveInWater();

            setFrictionOnVelocityInWater();

            SetAnimatorValues();
        }
        else
        {
            SetBool();

            Move();

            Jump();

            MapItems();

            setFrictionOnVelocity();

            SetAnimatorValues();
        }
    }

    private void SetBool()
    {
        booleens["isWalledLeft"] = !booleens["isGrounded"] && booleens["isWalledLeft"];
        booleens["isWalledRight"] = !booleens["isGrounded"] && booleens["isWalledRight"];
        booleens["isWalled"] = booleens["isWalledLeft"] || booleens["isWalledRight"];
        booleens["doubleJump"] = booleens["isGrounded"] || booleens["doubleJump"];

        if (rigidbody2D.velocity.x >= 0) movingSide = Direction.Right;
        else movingSide = Direction.Left;
    }

    private void Move()
    {
        movingDirection = Direction.None;
        if (Input.GetKey(KeyCode.D))
        {
            movingDirection = Direction.Right;
            spriteRenderer.flipX = false;
            if (!booleens["isWalledRight"] && rigidbody2D.velocity.x < maxRunSpeed) MoveOn(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movingDirection = Direction.Left;
            spriteRenderer.flipX = true;
            if (!booleens["isWalledLeft"] && rigidbody2D.velocity.x > -maxRunSpeed) MoveOn(Direction.Right);
        }
        else
        {
            SlowDown();
        }
    }
    
    private void MoveInWater()
    {
        movingDirection = Direction.None;
        if (Input.GetKey(KeyCode.D))
        {
            movingDirection = (Direction)((int)movingDirection + (int)Direction.Right);
            spriteRenderer.flipX = false;
            if (!booleens["isWalledRight"] && rigidbody2D.velocity.x < maxRunSpeed) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x + waterMoveStrengh * Time.deltaTime * 1, rigidbody2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movingDirection = (Direction)((int)movingDirection + (int)Direction.Left);
            spriteRenderer.flipX = true;
            if (!booleens["isWalledLeft"] && rigidbody2D.velocity.x > -maxRunSpeed) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x + waterMoveStrengh * Time.deltaTime * -1, rigidbody2D.velocity.y);
        }
        else
        {
            SlowDown();
        }

        if (Input.GetKey(KeyCode.W))
        {
            movingDirection = (Direction)((int)movingDirection + (int)Direction.Up);
            spriteRenderer.flipY = false;
            if (!booleens["isWalledRight"] && rigidbody2D.velocity.x < maxRunSpeed) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y + waterMoveStrengh * Time.deltaTime * 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movingDirection = (Direction)((int)movingDirection + (int)Direction.Down);
            spriteRenderer.flipY = true;
            if (!booleens["isWalledLeft"] && rigidbody2D.velocity.x > -maxRunSpeed) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y + waterMoveStrengh * Time.deltaTime * -1);
        }
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (booleens["doubleJump"] || booleens["isWalled"]))
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, jumpStrengh);

            if (!booleens["isGrounded"] && !booleens["isWalled"]) StartCoroutine(DoubleJump());
            else Jump_();
        }
    }

    private void SetAnimatorValues()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("YSpeed", rigidbody2D.velocity.y);
        animator.SetBool("IsGrounded", booleens["isGrounded"]);
    }

    private void MoveOn(Direction direction)
    {
        if (booleens["isGrounded"]) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - moveStrengh * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
        else rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - moveStrengh * airControl * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
    }

    private void MapItems()
    {
        if (rigidbody2D.velocity.y <= 0.01)
        {
            if (Input.GetKey(KeyCode.S)) SetPlateforms(false);
            else SetPlateforms(true);
        }
        else SetPlateforms(false);

        if (Input.GetKey(KeyCode.W))
        {
            if (booleens["Leader"])
            {
                rigidbody2D.velocity = new(rigidbody2D.velocity.x, leaderRideSpeed);
            }
        }
    }

    private void SlowDown()
    {
        if (booleens["isGrounded"])
        {
            rigidbody2D.velocity = new(rigidbody2D.velocity.x - (slowStrengh * Time.deltaTime) * (int)movingSide, rigidbody2D.velocity.y);
        }
    }

    private void Jump_()
    {
        if (booleens["isWalled"])
        {
            if (booleens["isWalledLeft"])
            {
                rigidbody2D.velocity = new(wallJumpStrengh, jumpStrengh);
                spriteRenderer.flipX = false;
            }
            if (booleens["isWalledRight"])
            {
                rigidbody2D.velocity = new(-wallJumpStrengh, jumpStrengh);
                spriteRenderer.flipX = true;
            }
        }
    }

    private IEnumerator DoubleJump()
    {
        animator.SetBool("IsDoubleJumping", true);
        booleens["doubleJump"] = false;
        Jump_();

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsDoubleJumping", false);
    }

    private void setFrictionOnVelocity()
    {
        Vector2 constraint;
        if (booleens["isGrounded"]) constraint = new(groundFriction * Time.deltaTime * rigidbody2D.velocity.x, 0);
        else constraint = new(airFriction * Time.deltaTime * rigidbody2D.velocity.x, airFriction * Time.deltaTime * rigidbody2D.velocity.y);
        rigidbody2D.velocity = new(rigidbody2D.velocity.x - constraint.x, rigidbody2D.velocity.y - constraint.y);

        if (booleens["isWalled"] && rigidbody2D.velocity.y < -wallRideDropSpeed) rigidbody2D.velocity = new(rigidbody2D.velocity.x, -wallRideDropSpeed);
    }

    private void setFrictionOnVelocityInWater()
    {
        Vector2 constraint;
        constraint = new(waterFriction * Time.deltaTime * rigidbody2D.velocity.x, waterFriction * Time.deltaTime * rigidbody2D.velocity.y);
        rigidbody2D.velocity = new(rigidbody2D.velocity.x - constraint.x, rigidbody2D.velocity.y - constraint.y);
    }

    private void SetPlateforms(bool enable)
    {
        GameObject[] plateforms = GameObject.FindGameObjectsWithTag("Plateform");
        foreach (GameObject plateform in plateforms)
        {
            var collider = plateform.GetComponent<CompositeCollider2D>();
            if (collider != null) collider.isTrigger = !enable;
        }
    }

    private void setRespawnPoint(Transform newTransform)
    {
        respawnPoint = newTransform;
    } 
    public void respawn()
    {
        transform.position = respawnPoint.position;
    }
}