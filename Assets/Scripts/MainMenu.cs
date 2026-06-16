using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public GameObject controlsPanel;

    void Start()
    {
        levelSelectPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenLevelSelect()
    {
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        levelSelectPanel.SetActive(false);
    }

    public void OpenControls()
    {
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("TotalDeaths", 0);
        PlayerPrefs.SetFloat("TotalTime", 0f);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
