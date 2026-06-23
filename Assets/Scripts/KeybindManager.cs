using System.Collections.Generic;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    public Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitKeys();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitKeys()
    {
        Keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftKey", "A")));
        Keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightKey", "D")));
        Keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey", "Space")));
    }

    public void BindKey(string keyName, KeyCode key)
    {
        if (key == KeyCode.Escape)
        {
            return;
        }

        Keys[keyName] = key;
        PlayerPrefs.SetString(keyName + "Key", key.ToString());
        PlayerPrefs.Save();
    }
}