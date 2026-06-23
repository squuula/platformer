using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public Button[] levelButtons;

    public Color passedColor = new Color(0.3f, 0.65f, 0.3f);
    public Color openedColor = new Color(0.5f, 0.5f, 0.5f);
    public Color lockedColor = new Color(0.7f, 0.2f, 0.2f);

    void OnEnable()
    {
        UpdateLevelSelectionUI();
    }

    public void UpdateLevelSelectionUI()
    {
        int maxOpenedLevel = PlayerPrefs.GetInt("MaxOpenedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;
            Image buttonImage = levelButtons[i].GetComponent<Image>();

            if (levelNumber < maxOpenedLevel)
            {
                levelButtons[i].interactable = true;
                if (buttonImage != null) buttonImage.color = passedColor;
            }

            else if (levelNumber == maxOpenedLevel)
            {
                levelButtons[i].interactable = true;
                if (buttonImage != null) buttonImage.color = openedColor;
            }

            else
            {
                levelButtons[i].interactable = false;
                if (buttonImage != null) buttonImage.color = lockedColor;
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}