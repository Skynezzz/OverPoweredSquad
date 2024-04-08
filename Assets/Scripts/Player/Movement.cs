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
    public float respirationTime;

    // UNITY FIELDS //
    //Float
    public float acceleration;
    public float jumpStrengh;
    public float wallJumpStrengh;
    public float airControl;
    public float maxSpeedX;
    public float maxSpeedY;
    public float wallRideDropSpeed;
    //Bool
    public bool doubleJump;
    public bool isGrounded;
    public bool isWalled;
    public bool isWalledLeft;
    public bool isWalledRight;

    // PRIVATE FIELDS //
    private float currentMaxSpeedX;
    private float currentMaxSpeedY;


    // FUNCTIONS //

    private void Start()
    {
        currentMaxSpeedX = maxSpeedX;
        currentMaxSpeedY = maxSpeedY;
    }

    // UPDATE //
    void Update()
    {
        isWalledLeft = !isGrounded && isWalledLeft;
        isWalledRight = !isGrounded && isWalledRight;
        isWalled = isWalledLeft || isWalledRight;
        doubleJump = isGrounded || doubleJump;


        // MOVE 
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (!isWalledRight) Move(Direction.Left);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (!isWalledLeft) Move(Direction.Right);
        }

        // JUMP //
        if (Input.GetKeyDown(KeyCode.Space) && (doubleJump || isWalled))
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, jumpStrengh);

            if (!isGrounded && !isWalled) StartCoroutine(DoubleJump());
            else Jump();
        }

        if (isWalled) currentMaxSpeedY = wallRideDropSpeed;
        else currentMaxSpeedY = maxSpeedY;
        LimitXVelocity();
        LimitYVelocity();
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("YSpeed", rigidbody2D.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void Move(Direction direction)
    {
        if (isGrounded) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - acceleration * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
        else rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - acceleration * airControl * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
    }

    void Jump()
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
        Jump();

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsDoubleJumping", false);
    }

    public void LimitXVelocity()
    {
        if (Math.Abs(rigidbody2D.velocity.x) > currentMaxSpeedX) rigidbody2D.velocity = new Vector3((rigidbody2D.velocity.x / Math.Abs(rigidbody2D.velocity.x)) * currentMaxSpeedX, rigidbody2D.velocity.y);
    }

    public void LimitYVelocity()
    {
        if (rigidbody2D.velocity.y < -currentMaxSpeedY) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, -currentMaxSpeedY);
    }
}