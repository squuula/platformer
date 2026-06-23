using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float deathTimer = 1f;
    private float horizontalInput;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;
    private bool isGrounded;

    public int emeraldsCount = 0;

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;
    private bool isDead = false;
    private int totalEmeraldsInLevel;

    private Vector3 startPosition;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        startPosition = transform.position;

        PlayerPrefs.SetInt("TotalDeaths", 0);
        PlayerPrefs.Save();

        totalEmeraldsInLevel = GameObject.FindGameObjectsWithTag("Emerald").Length;
        UIManager.Instance.UpdateEmeraldsUI(emeraldsCount, totalEmeraldsInLevel);
    }

    public bool HasCollectedAllEmeralds()
    {
        return emeraldsCount >= totalEmeraldsInLevel;
    }

    void Update()
    {
        if (isDead) return;

        horizontalInput = 0f;

        if (Input.GetKey(KeybindManager.Instance.Keys["Left"]))
        {
            horizontalInput = -1f;
        }
        if (Input.GetKey(KeybindManager.Instance.Keys["Right"]))
        {
            horizontalInput = 1f;
        }

        anim.SetFloat("speed", Mathf.Abs(horizontalInput));

        if (Input.GetKeyDown(KeybindManager.Instance.Keys["Jump"]) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (horizontalInput > 0 && !facingRight) Flip();
        else if (horizontalInput < 0 && facingRight) Flip();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Emerald"))
        {
            emeraldsCount++;
            UIManager.Instance.UpdateEmeraldsUI(emeraldsCount, totalEmeraldsInLevel);

            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Trap") && !isDead)
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        int savedDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        savedDeaths++;
        PlayerPrefs.SetInt("TotalDeaths", savedDeaths);
        PlayerPrefs.Save();

        anim.SetTrigger("die");

        yield return new WaitForSeconds(deathTimer);

        ResetLevelObjects();

        transform.position = startPosition;
        rb.simulated = true;
        if (sprite != null) sprite.enabled = true;
        if (anim != null) anim.enabled = true;

        anim.Rebind();
        isDead = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    void ResetLevelObjects()
    {
        emeraldsCount = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateEmeraldsUI(emeraldsCount, totalEmeraldsInLevel);
        }

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == gameObject.scene.name)
            {
                if (obj.CompareTag("Emerald"))
                {
                    obj.SetActive(true);
                }

                else if (obj.TryGetComponent<CrumblingPlatform>(out CrumblingPlatform platform))
                {
                    platform.ResetPlatform();
                }
            }
        }
    }
}