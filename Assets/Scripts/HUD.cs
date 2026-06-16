using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI emeraldText;

    float timer = 0f;
    bool running = true;

    void Start()
    {
        timer = PlayerPrefs.GetFloat("TotalTime", 0f);

        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        if (levelText != null)
            levelText.text = "Уровень " + levelNumber;

        UpdateDeathText();
        UpdateTimerText();
    }

    void Update()
    {
        if (!running || PauseMenu.IsPaused) return;

        timer += Time.deltaTime;
        PlayerPrefs.SetFloat("TotalTime", timer);
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddDeath()
    {
        int deaths = PlayerPrefs.GetInt("TotalDeaths", 0) + 1;
        PlayerPrefs.SetInt("TotalDeaths", deaths);
        PlayerPrefs.Save();
        UpdateDeathText();
    }

    void UpdateDeathText()
    {
        int deaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        if (deathCountText != null)
            deathCountText.text = "Смерти: " + deaths;
    }

    public void UpdateEmeralds(int collected, int total)
    {
        if (emeraldText != null)
            emeraldText.text = $"{collected}/{total}";
    }

    public void StopTimer() => running = false;
}
