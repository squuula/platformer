using UnityEngine;

/// <summary>
/// Повесить на каждый коллектибл (звёздочка, фрукт и т.д.)
/// Коллайдер → Is Trigger = true
/// </summary>
public class Collectible : MonoBehaviour
{
    [Header("Optional FX")]
    public GameObject collectFXPrefab; // партикл или анимация при сборе

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Уведомляем менеджер
        CollectibleManager.Instance?.OnCollect();

        // Эффект (опционально)
        if (collectFXPrefab)
            Instantiate(collectFXPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
