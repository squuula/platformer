using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public int maxJumps = 2;

    [Header("Wall")]
    public float wallSlideSpeed = 1.5f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;
    public LayerMask wallLayer;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Wall Check")]
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public float wallCheckRadius = 0.15f;

    [Header("One Way Platforms")]
    public LayerMask oneWayLayer;

    Rigidbody2D rb;
    Collider2D col;
    int jumpsLeft;
    bool isGrounded;
    bool isTouchingWallRight;
    bool isTouchingWallLeft;
    bool isWallSliding;
    bool isDashing;
    bool canDash = true;
    float dashCooldownTimer;
    float horizontalInput;
    bool facingRight = true;

    bool jumpPressed;
    bool dashPressed;
    bool dropPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed) jumpPressed = true;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed) dashPressed = true;
    }

    public void OnDrop(InputValue value)
    {
        if (value.isPressed) dropPressed = true;
    }

    void Update()
    {
        if (isDashing) return;

        CheckGroundAndWalls();
        HandleFlip();
        HandleJump();
        HandleDash();
        HandleWallSlide();
        HandleDropThrough();

        jumpPressed = false;
        dashPressed = false;
        dropPressed = false;
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        if (!isWallSliding)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,
                Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
    }

    void CheckGroundAndWalls()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, wallLayer);
        isTouchingWallLeft  = Physics2D.OverlapCircle(wallCheckLeft.position,  wallCheckRadius, wallLayer);

        if (isGrounded)
        {
            jumpsLeft = maxJumps;
            canDash = true;
        }
    }

    void HandleJump()
    {
        if (!jumpPressed) return;

        if (isWallSliding)
        {
            float dir = isTouchingWallRight ? -1f : 1f;
            rb.linearVelocity = new Vector2(dir * wallJumpForceX, wallJumpForceY);
            jumpsLeft = maxJumps - 1;
            return;
        }

        if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }
    }

    void HandleWallSlide()
    {
        bool touchingWall = (isTouchingWallRight && horizontalInput > 0)
                         || (isTouchingWallLeft  && horizontalInput < 0);

        isWallSliding = !isGrounded && touchingWall && rb.linearVelocity.y < 0;

        if (isWallSliding)
            jumpsLeft = maxJumps;
    }

    void HandleDash()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (dashPressed && canDash && dashCooldownTimer <= 0)
            StartCoroutine(DashRoutine());
    }

    System.Collections.IEnumerator DashRoutine()
    {
        isDashing = true;
        canDash = false;
        dashCooldownTimer = dashCooldown;

        float dir = facingRight ? 1f : -1f;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(dir * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = 1f;
        isDashing = false;
    }

    void HandleDropThrough()
    {
        if (!dropPressed) return;

        // Ищем все OneWay платформы под персонажем
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            groundCheck.position, Vector2.down, groundCheckRadius * 2f, oneWayLayer);

        foreach (var hit in hits)
        {
            var effector = hit.collider.GetComponent<PlatformEffector2D>();
            if (effector != null)
                StartCoroutine(DropRoutine(effector));
        }
    }

    System.Collections.IEnumerator DropRoutine(PlatformEffector2D effector)
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.35f);
        effector.rotationalOffset = 0f;
    }

    void HandleFlip()
    {
        if (horizontalInput > 0 && !facingRight) Flip();
        else if (horizontalInput < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheckRight)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        }
        if (wallCheckLeft)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        }
    }
}