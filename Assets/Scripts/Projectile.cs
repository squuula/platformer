using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 8f;
    public float lifetime = 5f;     // авто-уничтожение через N секунд
    public Vector2 direction = Vector2.left; // направление полёта

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Start()
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Убиваем персонажа
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.Die();
            Destroy(gameObject);
            return;
        }

        // Уничтожаемся о стены (слой Ground)
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
