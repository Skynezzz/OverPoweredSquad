using System;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    // ATTACH GAMEOBJECT //
    public Transform leftGroundedPoint;
    public Transform rightGroundedPoint;
    public Transform bottomLeftWalledPoint;
    public Transform topLeftWalledPoint;
    public Transform bottomRightWalledPoint;
    public Transform topRightWalledPoint;

    // COMPONENTS //
    public Rigidbody2D rigidbody2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // UNITY FIELDS //
    public float acceleration;
    public float maxSpeedX;
    public float maxSpeedY;
    public float wallRideSpeedY;
    public float airControl;
    public float jumpStrengh;
    public float wallJumpStrengh;

    public bool doubleJump;
    public bool isGrounded;
    public bool isWalled;
    public bool isWalledLeft;
    public bool isWalledRight;

    // PRIVATE FIELDS //
    private bool isDoubleJumping = false;
    private const float DOUBLE_JUMP_DURATION = 0.5f;
    private float doubleJumpTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapArea(leftGroundedPoint.position, rightGroundedPoint.position);
        isWalledLeft = !isGrounded && Physics2D.OverlapArea(bottomLeftWalledPoint.position, topLeftWalledPoint.position);
        isWalledRight = !isGrounded && Physics2D.OverlapArea(bottomRightWalledPoint.position, topRightWalledPoint.position);
        isWalled = isWalledLeft || isWalledRight;
        doubleJump = isGrounded || doubleJump;

        if (doubleJumpTime < 0)
        {
            doubleJumpTime = DOUBLE_JUMP_DURATION;
            animator.SetBool("IsDoubleJumping", false);
            isDoubleJumping = false;
        }
        else doubleJumpTime -= Time.deltaTime;

        // MOVE //
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
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, jumpStrengh);
            doubleJump = false;

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

            if (!isGrounded && !isWalled)
            {
                animator.SetBool("IsDoubleJumping", true);
                isDoubleJumping = true;
            }
        }

        LimitXVelocity();
        LimitYVelocity();
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("YSpeed", rigidbody2D.velocity.y);
        animator.SetBool("IsGrounded", isGrounded );
        animator.SetBool("IsFalling", !isGrounded && rigidbody2D.velocity.y < 0);
        animator.SetBool("IsJumping", !isGrounded && rigidbody2D.velocity.y > 0);
    }

    public void Move(Direction direction)
    {
        if (isGrounded) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - acceleration * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
        else rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - acceleration * airControl * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
    }

    public void LimitXVelocity()
    {
        if (Math.Abs(rigidbody2D.velocity.x) > maxSpeedX) rigidbody2D.velocity = new Vector3((rigidbody2D.velocity.x / Math.Abs(rigidbody2D.velocity.x)) * maxSpeedX, rigidbody2D.velocity.y);
    }

    public void LimitYVelocity()
    {
        if (Math.Abs(rigidbody2D.velocity.y) > maxSpeedY) rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, (rigidbody2D.velocity.y / Math.Abs(rigidbody2D.velocity.y)) * maxSpeedY);
    }
}