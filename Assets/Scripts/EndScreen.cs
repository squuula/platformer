using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    void Start()
    {
        int deaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        float time = PlayerPrefs.GetFloat("TotalTime", 0f);
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        if (statsText != null)
            statsText.text = $"Смертей: {deaths}\nВремя: {minutes:00}:{seconds:00}";
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
