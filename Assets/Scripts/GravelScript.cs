using UnityEngine;
using System.Collections;

public class CrumblingPlatform : MonoBehaviour
{
    public float delayBeforeFall = 0.5f;
    public float destroyDelay = 10f;
    public float fallGravityScale = 4f;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private bool isFalling = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        InitPlatform();
    }

    void InitPlatform()
    {
        isFalling = false;

        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            if (collision.contacts[0].normal.y < -0.5f)
            {
                StartCoroutine(FallRoutine());
            }
        }
    }

    IEnumerator FallRoutine()
    {
        isFalling = true;

        yield return new WaitForSeconds(delayBeforeFall);

        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = fallGravityScale;
        }

        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }

    public void ResetPlatform()
    {
        StopAllCoroutines();

        transform.position = startPosition;
        transform.rotation = startRotation;

        gameObject.SetActive(true);
        InitPlatform();
    }
}