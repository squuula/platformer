using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool IsPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
