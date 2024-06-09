using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;

    private Rigidbody2D rB2D;
    private Vector2 movementInput;
    private float jumpForce;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField] private float speed;
    [SerializeField] private Transform checkGroundTR;
    [SerializeField] private float groundCheckerRadius;
    [SerializeField] private LayerMask isGround;

    void Start()
    {
        rB2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        jumpForce = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        Jump();

        playerAnimator.SetFloat("VelX", Mathf.Abs(rB2D.velocity.x));
        playerAnimator.SetFloat("VelY", rB2D.velocity.y);
        playerAnimator.SetBool("IsGrounded", IsGrounded());
    }

    private void Walk()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal") * speed, rB2D.velocity.y);

        if (movementInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    private void Jump()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rB2D.velocity = new Vector2(rB2D.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Space) && rB2D.velocity.y > 0f)
        {
            rB2D.velocity = new Vector2(rB2D.velocity.x, rB2D.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        rB2D.velocity = movementInput;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(checkGroundTR.position, groundCheckerRadius, isGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(checkGroundTR.position, groundCheckerRadius);
    }
}
