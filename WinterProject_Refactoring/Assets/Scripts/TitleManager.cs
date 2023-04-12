using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private InputField NameInputField;

    private void Start()
    {
        NameInputField.onEndEdit.AddListener(InputEnter);
    }

    void InputEnter(string value)
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.saveData.PlayerName = value;
            GameManager.Instance.SaveData();
            SceneManager.LoadScene("MainStory");
        }
    }
}
