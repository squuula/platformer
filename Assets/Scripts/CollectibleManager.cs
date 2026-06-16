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

    public bool AllCollected => collected >= totalCollectibles && totalCollectibles > 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        totalCollectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        Debug.Log($"Всего коллектиблов: {totalCollectibles}");
        FindObjectOfType<HUD>()?.UpdateEmeralds(collected, totalCollectibles);
    }

    public void OnCollect()
    {
        collected++;
        FindObjectOfType<HUD>()?.UpdateEmeralds(collected, totalCollectibles);

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
