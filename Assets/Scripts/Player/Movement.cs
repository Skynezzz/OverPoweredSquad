using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1
    }


    // FIELDS //

    // PUBLIC UNITY //
    //Float
    public float moveStrengh;
    public float slowStrengh;
    public float maxRunSpeed;
    public float jumpStrengh;
    public float wallJumpStrengh;
    public float airControl;
    public float groundFriction;
    public float airFriction;
    public float wallRideDropSpeed;

    public float dashStrenght;
    public float dashMultiplicater;
    //Bool
    public Dictionary<string, bool> booleens;

    // PRIVATE //

    // COMPONENTS //
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // FIELDS //
    private Direction movingSide;


    // FUNCTIONS //

    private void Start()
    {
        booleens = new Dictionary<string, bool>
        {
            { "doubleJump", false },
            { "isGrounded", false },
            { "isWalled", false },
            { "isWalledLeft", false },
            { "isWalledRight", false },
            { "isLeadered", false },
            { "dash", false }
        };



        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetBool();

        Move();

        Jump();

        MapItems();

        setFrictionOnVelocity();

        SetAnimatorValues();
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
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (!booleens["isWalledRight"] && rigidbody2D.velocity.x < maxRunSpeed) MoveOn(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (!booleens["isWalledLeft"] && rigidbody2D.velocity.x > -maxRunSpeed) MoveOn(Direction.Right);
        }
        else
        {
            SlowDown();
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

    private void SetPlateforms(bool enable)
    {
        GameObject[] plateforms = GameObject.FindGameObjectsWithTag("Plateform");
        foreach (GameObject plateform in plateforms)
        {
            var collider = plateform.GetComponent<CompositeCollider2D>();
            if (collider != null) collider.isTrigger = !enable;
        }
    }
}