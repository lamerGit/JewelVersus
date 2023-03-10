using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSetUI : MonoBehaviour
{
    TextMeshProUGUI winnerText;
    Button mainButton;
    Button reGameButton;


    private void Awake()
    {
        winnerText=transform.Find("WinnerText").GetComponent<TextMeshProUGUI>();
        mainButton=transform.Find("MainMenuButton").GetComponent<Button>();
        reGameButton=transform.Find("ReGameButton").GetComponent<Button>();

        mainButton.onClick.AddListener(OnMainMenu);
        reGameButton.onClick.AddListener(OnReGame);

    }


    void OnMainMenu()
    {
        SceneManager.LoadScene((int)SceneEnum.Robby);
    }

    void OnReGame()
    {
        SceneManager.LoadScene((int)SceneEnum.VsAI);
    }

    public void Open(WinnerEnum winner)
    {
        gameObject.SetActive(true);

        if (winner==WinnerEnum.None)
        {
            winnerText.text = "무승부";
        }else if(winner==WinnerEnum.AI)
        {
            winnerText.text = "AI승리";
        } else if(winner==WinnerEnum.Player) {
            winnerText.text = "플레이어 승리";
        
        }else if(winner==WinnerEnum.Player2)
        {
            winnerText.text = "플레이어2 승리";
        }
    }

    

    public void Close()
    {
        gameObject.SetActive(false);
    }

    
}
