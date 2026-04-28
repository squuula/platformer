using UnityEngine;

/// <summary>
/// Повесить на объект-стену/турель. Указать Projectile prefab и направление.
/// </summary>
public class ProjectileSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject projectilePrefab;
    public float fireInterval = 2f;     // секунды между выстрелами
    public float firstShotDelay = 0.5f; // задержка первого выстрела

    [Header("Direction & Offset")]
    public Vector2 shootDirection = Vector2.left;
    public Vector2 spawnOffset = new Vector2(-0.5f, 0f);

    void Start()
    {
        InvokeRepeating(nameof(Shoot), firstShotDelay, fireInterval);
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = transform.position + (Vector3)(spawnOffset);
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Передаём направление в снаряд
        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
            projectile.direction = shootDirection;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position + (Vector3)spawnOffset;
        Gizmos.DrawLine(start, start + (Vector3)(shootDirection.normalized * 1.5f));
        Gizmos.DrawWireSphere(start, 0.15f);
    }
}
