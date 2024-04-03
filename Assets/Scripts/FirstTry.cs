using System;
using System.Threading;
using UnityEngine;

public class FirstTry : MonoBehaviour
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

    // IN UNITY FIELDS //
    public float acceleration;
    public float maxSpeed;
    public float airControl;
    public float jumpStrengh;
    public float wallJumpStrengh;

    public bool doubleJump;
    public bool isGrounded;
    public bool isWalledLeft;
    public bool isWalledRight;

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
        doubleJump = isGrounded || isWalledLeft || isWalledRight || doubleJump;

        animator.SetBool("IsDoubleJumping", !doubleJump && rigidbody2D.velocity.y > 0);
        if (isGrounded) animator.SetBool("IsDoubleJumping", false);

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
            if (!isGrounded || isWalledLeft || isWalledRight)
            {
                animator.SetBool("IsDoubleJumping", true);
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

        LimitVelocity();
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

    public void LimitVelocity()
    {
        if (Math.Abs(rigidbody2D.velocity.x) > maxSpeed) rigidbody2D.velocity = new Vector3((rigidbody2D.velocity.x / Math.Abs(rigidbody2D.velocity.x)) * maxSpeed, rigidbody2D.velocity.y);
    }
}