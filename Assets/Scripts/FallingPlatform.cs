using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingPlatform : MonoBehaviour
{
    [Header("Timing")]
    [Tooltip("Задержка перед падением после наступания (секунды)")]
    public float fallDelay = 0.8f;

    [Header("Shake")]
    [Tooltip("Амплитуда тряски перед падением")]
    public float shakeAmount = 0.05f;

    Rigidbody2D rb;
    Vector3 startPos;
    bool triggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                triggered = true;
                StartCoroutine(FallRoutine());
                return;
            }
        }
    }

    IEnumerator FallRoutine()
    {
        float elapsed = 0f;
        while (elapsed < fallDelay)
        {
            float offsetX = Mathf.Sin(elapsed * 60f) * shakeAmount;
            transform.position = startPos + new Vector3(offsetX, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;

        rb.bodyType = RigidbodyType2D.Dynamic;
        
    }
}