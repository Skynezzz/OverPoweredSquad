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
    public float jumpStrengh;

    public bool isGrounded;
    public bool isWalled;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapArea(leftGroundedPoint.position, rightGroundedPoint.position);
        isWalled = !isGrounded && (Physics2D.OverlapArea(bottomLeftWalledPoint.position, topLeftWalledPoint.position) || Physics2D.OverlapArea(bottomRightWalledPoint.position, topRightWalledPoint.position));

        // MOVE //
        if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (!isWalled) Move(Direction.Left);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (!isWalled) Move(Direction.Right);
        }
        LimitVelocity();
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

        // JUMP //
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, jumpStrengh);
        }
    }

    public void Move(Direction direction)
    {
        rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x - acceleration * Time.deltaTime * (int)direction, rigidbody2D.velocity.y);
    }

    public void LimitVelocity()
    {
        if (Math.Abs(rigidbody2D.velocity.x) > maxSpeed) rigidbody2D.velocity = new Vector3((rigidbody2D.velocity.x / Math.Abs(rigidbody2D.velocity.x)) * maxSpeed, rigidbody2D.velocity.y);
    }
}