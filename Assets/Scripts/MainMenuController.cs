using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject optionsPanel;

    public Slider volumeSlider;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public TMP_Text leftText;
    public TMP_Text rightText;
    public TMP_Text jumpText;

    private string currentBindingKeyName = "";

    void Start()
    {
        SetupAudio();
        SetupResolutions();
        UpdateKeybindButtonsText();

        if (PlayerPrefs.GetInt("OpenLevelSelectDirectly", 0) == 1)
        {
            PlayerPrefs.SetInt("OpenLevelSelectDirectly", 0);
            PlayerPrefs.Save();

            if (levelSelectPanel != null)
            {
                levelSelectPanel.SetActive(true);
            }

            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
            }

            LevelSelectionManager levelManager = FindFirstObjectByType<LevelSelectionManager>();
            if (levelManager != null)
            {
                levelManager.UpdateLevelSelectionUI();
            }
        }
    }

    void SetupAudio()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 1f);
            AudioListener.volume = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRateRatio.value.ToString("0") + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void UpdateKeybindButtonsText()
    {
        if (KeybindManager.Instance != null)
        {
            leftText.text = KeybindManager.Instance.Keys["Left"].ToString();
            rightText.text = KeybindManager.Instance.Keys["Right"].ToString();
            jumpText.text = KeybindManager.Instance.Keys["Jump"].ToString();
        }
    }

    public void StartBinding(string keyName)
    {
        currentBindingKeyName = keyName;
        if (keyName == "Left") leftText.text = "<   >";
        if (keyName == "Right") rightText.text = "<   >";
        if (keyName == "Jump") jumpText.text = "<   >";
    }

    void OnGUI()
    {
        if (currentBindingKeyName == "") return;

        Event e = Event.current;
        if (e != null && e.isKey)
        {
            KeyCode pressedKey = e.keyCode;

            if (pressedKey == KeyCode.None) return;

            foreach (var bind in KeybindManager.Instance.Keys)
            {
                if (bind.Value == pressedKey && bind.Key != currentBindingKeyName)
                {
                    KeyCode oldKeyForCurrentAction = KeybindManager.Instance.Keys[currentBindingKeyName];

                    KeybindManager.Instance.BindKey(bind.Key, oldKeyForCurrentAction);

                    break;
                }
            }

            KeybindManager.Instance.BindKey(currentBindingKeyName, pressedKey);

            currentBindingKeyName = "";
            UpdateKeybindButtonsText();
        }
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("GameVolume", value);
    }

    public void OpenMainMenu() { TogglePanels(true, false, false); }
    public void OpenLevelSelect() { TogglePanels(false, true, false); }
    public void OpenOptions() { TogglePanels(false, false, true); }

    void TogglePanels(bool main, bool select, bool opt)
    {
        mainMenuPanel.SetActive(main);
        levelSelectPanel.SetActive(select);
        optionsPanel.SetActive(opt);
    }

    public void QuitGame() { Application.Quit(); }
    public void LoadLevel(string levelName) { SceneManager.LoadScene(levelName); }

    public GameObject resetConfirmationPanel;

    public void OpenResetConfirmation()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(true);
        }
    }

    public void CloseResetConfirmation()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(false);
        }
    }

    public void ConfirmResetProgress()
    {
        PlayerPrefs.SetInt("MaxOpenedLevel", 1);

        PlayerPrefs.SetInt("TotalDeaths", 0);

        PlayerPrefs.Save();

        CloseResetConfirmation();

        LevelSelectionManager levelManager = FindFirstObjectByType<LevelSelectionManager>();
        if (levelManager != null)
        {
            levelManager.UpdateLevelSelectionUI();
        }
    }
}