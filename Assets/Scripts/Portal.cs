using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Повесить на портал. Коллайдер → Is Trigger = true.
/// </summary>
public class Portal : MonoBehaviour
{
    [Header("Settings")]
    public string nextLevelScene;
    public float delay = 3f;

    bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        used = true;
        StartCoroutine(LoadRoutine());
    }

    IEnumerator LoadRoutine()
    {
        yield return new WaitForSeconds(delay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentIndex >= unlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentIndex + 1);
            PlayerPrefs.Save();
        }

        if (!string.IsNullOrEmpty(nextLevelScene))
            SceneManager.LoadScene(nextLevelScene);
        else
            SceneManager.LoadScene(currentIndex + 1);
    }
}
