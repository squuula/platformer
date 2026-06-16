using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Повесить на отдельный GameObject "GameManager".
/// В Inspector:
///   PlayerHealth.onDeath → GameManager.RestartLevel
///   CollectibleManager.onLevelComplete → GameManager.LevelComplete
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Задержка перед рестартом (секунды)")]
    public float restartDelay = 1f;

    [Tooltip("Имя сцены следующего уровня (пусто = та же сцена)")]
    public string nextLevelScene = "";

    // ────────────────────────────────────
    // Смерть персонажа → рестарт
    // ────────────────────────────────────
    public void RestartLevel()
    {
        Debug.Log("Перезапуск уровня...");
        Invoke(nameof(DoRestart), restartDelay);
    }

    void DoRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ────────────────────────────────────
    // Все коллектиблы собраны
    // ────────────────────────────────────
    public void LevelComplete()
    {
        Debug.Log("Уровень пройден!");

        if (!string.IsNullOrEmpty(nextLevelScene))
            Invoke(nameof(LoadNext), 1.5f);
        else
            Invoke(nameof(DoRestart), 2f); // повтор того же уровня для демо
    }

    void LoadNext()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        // currentIndex - 1 потому что MainMenu занимает индекс 0
        int currentLevel = currentIndex;
        if (currentLevel >= unlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene(nextLevelScene);
    }
}
