using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button[] levelButtons; // кнопки уровней по порядку

    [Header("Level Scene Names")]
    public string[] levelNames; // названия сцен: level1, level2 и т.д.

    void OnEnable()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool isUnlocked = (i + 1) <= unlockedLevel;
            bool isCompleted = (i + 1) < unlockedLevel;

            levelButtons[i].interactable = isUnlocked;

            var img = levelButtons[i].GetComponent<Image>();
            if (img != null)
            {
                if (isCompleted)
                    img.color = new Color(0.2f, 0.8f, 0.2f); // зелёный
                else if (isUnlocked)
                    img.color = Color.white;
                else
                    img.color = new Color(0.8f, 0.2f, 0.2f); // красный
            }
        }
    }

    public void LoadLevel(int index)
    {
        Debug.Log($"LoadLevel called: index={index}, name={( index < levelNames.Length ? levelNames[index] : "out of range")}");
        if (index < levelNames.Length)
            SceneManager.LoadScene(levelNames[index]);
    }
}
