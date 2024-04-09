using System;
using System.Collections;
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

    // COMPONENTS //
    public Rigidbody2D rigidbody2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // POWERS //
    public float jumpMultiplicater;
    public float dashMultiplicater;
    public float respirationTime;

    // UNITY FIELDS //
    //Float
    public float moveStrengh;
    public float slowStrengh;
    public float maxRunSpeed;
    public float jumpStrengh;
    public float dashStrenght;
    public float wallJumpStrengh;
    public float airControl;
    public float groundFriction;
    public float airFriction;
    public float wallRideDropSpeed;
    //Bool
    public bool doubleJump;
    public bool dash;
    public bool isGrounded;
    public bool isWalled;
    public bool isWalledLeft;
    public bool isWalledRight;

    // PRIVATE FIELDS //
    private Direction movingSide;


    // FUNCTIONS //

    void Update()
    {
        SetBool();

        Move();

        Jump();

        setFrictionOnVelocity();

        SetAnimatorValues();
    }

    private void SetBool()
    {
        isWalledLeft = !isGrounded && isWalledLeft;
        isWalledRight = !isGrounded && isWalledRight;
        isWalled = isWalledLeft || isWalledRight;
        doubleJump = isGrounded || doubleJump;

        if (rigidbody2D.velocity.x >= 0) movingSide = Direction.Right;
        else movingSide = Direction.Left;
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (!isWalledRight && rigidbody2D.velocity.x < maxRunSpeed) MoveOn(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (!isWalledLeft && rigidbody2D.velocity.x > -maxRunSpeed) MoveOn(Direction.Right);
        }
        else
        {
            SlowDown();
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (doubleJump || isWalled))
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, jumpStrengh);

            if (!isGrounded && !isWalled) StartCoroutine(DoubleJump());
            else Jump_();
        }
    }
    
    private void SetAnimatorValues()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("YSpeed", rigidbody2D.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void MoveOn(Direction direction)
    {
        if (isGrounded) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - moveStrengh * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
        else rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - moveStrengh * airControl * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
    }

    public void SlowDown()
    {
        if (isGrounded)
        {
            rigidbody2D.velocity = new(rigidbody2D.velocity.x - (slowStrengh * Time.deltaTime) * (int)movingSide, rigidbody2D.velocity.y);
        }
    }

    void Jump_()
    {
        if (isWalled)
        {
            if (isWalledLeft)
            {
                rigidbody2D.velocity = new(wallJumpStrengh, jumpStrengh);
                spriteRenderer.flipX = false;
            }
            if (isWalledRight)
            {
                rigidbody2D.velocity = new(-wallJumpStrengh, jumpStrengh);
                spriteRenderer.flipX = true;
            }
        }
    }

    private IEnumerator DoubleJump()
    {
        animator.SetBool("IsDoubleJumping", true);
        doubleJump = false;
        Jump_();

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsDoubleJumping", false);
    }

    public void setFrictionOnVelocity()
    {
        Vector2 constraint;
        if (isGrounded) constraint = new(groundFriction * Time.deltaTime * rigidbody2D.velocity.x, 0);
        else constraint = new(airFriction * Time.deltaTime * rigidbody2D.velocity.x, airFriction * Time.deltaTime * rigidbody2D.velocity.y);
        rigidbody2D.velocity = new(rigidbody2D.velocity.x - constraint.x, rigidbody2D.velocity.y - constraint.y);

        if (isWalled && rigidbody2D.velocity.y < -wallRideDropSpeed) rigidbody2D.velocity = new(rigidbody2D.velocity.x, -wallRideDropSpeed);
    }
}