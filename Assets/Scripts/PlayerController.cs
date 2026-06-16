using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;

    [Header("Jump")]
    public float jumpForce = 14f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Animator anim;
    bool isGrounded;
    float horizontalInput;
    bool facingRight = true;
    bool jumpPressed;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressed = true;

        HandleFlip();
        HandleJump();
        UpdateAnimator();
        jumpPressed = false;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (jumpPressed && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void UpdateAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    public void PlayDeath()
    {
        isDead = true;
        anim.SetBool("IsDead", true);
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
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}