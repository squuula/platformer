using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameplayHUD;
    public TMP_Text emeraldsText;
    public TMP_Text timerText;

    public TMP_Text finalTimerText;
    public TMP_Text finalDeathsText;

    private float timeElapsed = 0f;
    private bool isTimerRunning = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void UpdateEmeraldsUI(int current, int total)
    {
        if (emeraldsText != null)
        {
            emeraldsText.text = $"{current} / {total}";
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = $"{GetFormattedTime()}";
        }
    }

    string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowFinalResults(int currentLevelNumber)
    {
        isTimerRunning = false;

        if (gameplayHUD != null)
        {
            gameplayHUD.SetActive(false);
        }

        if (finalTimerText != null)
        {
            finalTimerText.text = $"Итоговое время: {GetFormattedTime()}";
        }

        if (finalDeathsText != null)
        {
            int totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            finalDeathsText.text = $"{totalDeaths}";
        }

        int maxOpenedLevel = PlayerPrefs.GetInt("MaxOpenedLevel", 1);

        if (currentLevelNumber == maxOpenedLevel)
        {
            PlayerPrefs.SetInt("MaxOpenedLevel", currentLevelNumber + 1);
            PlayerPrefs.Save();
        }
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        PlayerPrefs.DeleteKey("TotalDeaths");
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void HideGameplayHUD()
    {
        isTimerRunning = false;
        if (gameplayHUD != null)
        {
            gameplayHUD.SetActive(false);
        }
    }

    public void GoToLevelSelection(string mainMenuSceneName)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);

        PlayerPrefs.SetInt("OpenLevelSelectDirectly", 1);
        PlayerPrefs.Save();
    }
}