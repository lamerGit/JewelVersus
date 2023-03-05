using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginFailUI : MonoBehaviour
{
    Button _okButton;
    private void Awake()
    {
        _okButton=transform.Find("OkBtn").GetComponent<Button>();

        _okButton.onClick.AddListener(GameQuit);

    }

    
    public void Open()
    {
        gameObject.SetActive(true);
    }
    void GameQuit()
    {
        Application.Quit();
    }
}
