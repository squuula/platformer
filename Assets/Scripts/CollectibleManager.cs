using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton. Повесить на один GameObject на сцене.
/// Автоматически считает все Collectible на сцене в Start().
/// </summary>
public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    [Header("Events")]
    public UnityEvent onLevelComplete; // → GameManager.LevelComplete()

    int totalCollectibles;
    int collected;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Считаем все коллектиблы на сцене
        totalCollectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        Debug.Log($"Всего коллектиблов: {totalCollectibles}");
    }

    public void OnCollect()
    {
        collected++;
        Debug.Log($"Собрано: {collected} / {totalCollectibles}");

        if (collected >= totalCollectibles)
        {
            Debug.Log("Уровень пройден!");
            onLevelComplete.Invoke();
        }
    }

    /// <summary>Сброс при рестарте уровня</summary>
    public void Reset()
    {
        collected = 0;
        totalCollectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
    }
}
