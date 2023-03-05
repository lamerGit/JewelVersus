using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VsPlayerGameSetUI : MonoBehaviour
{
    TextMeshProUGUI winnerText;
    Button mainButton;
    Button reGameButton;

    TextMeshProUGUI _reGameText;

    private void Awake()
    {
        winnerText = transform.Find("WinnerText").GetComponent<TextMeshProUGUI>();
        mainButton = transform.Find("MainMenuButton").GetComponent<Button>();
        reGameButton = transform.Find("ReGameButton").GetComponent<Button>();

        _reGameText = transform.Find("ReGameText").GetComponent<TextMeshProUGUI>();

        _reGameText.gameObject.SetActive(false);

        mainButton.onClick.AddListener(OnMainMenu);
        reGameButton.onClick.AddListener(OnReGame);

    }


    void OnMainMenu()
    {
        C_EnterRobby loginPacket = new C_EnterRobby();
        loginPacket.UniqueId = Managers.Network.Token;
        Managers.Network.Send(loginPacket);
        SceneManager.LoadScene((int)SceneEnum.Robby);
    }

    void OnReGame()
    {
        Debug.Log("재경기 요청");
        C_ResetGame resetPacket= new C_ResetGame();
        Managers.Network.Send(resetPacket);

        ReGameRequest();
    }


    public void VsWinnerPlayerOpen()
    {
        gameObject.SetActive(true);
        _reGameText.gameObject.SetActive(false);
        GameManager.Instance.VsMyPlayerBlockManager.GameStart = false;
        winnerText.text = "당신의 승리";

    }

    public void VsLosePlayerOpen()
    {
        gameObject.SetActive(true);
        _reGameText.gameObject.SetActive(false);
        GameManager.Instance.VsMyPlayerBlockManager.GameStart = false;
        winnerText.text = "당신의 패배";

    }

    public void VsDrowPlayerOpen()
    {
        gameObject.SetActive(true);
        _reGameText.gameObject.SetActive(false);
        GameManager.Instance.VsMyPlayerBlockManager.GameStart = false;
        winnerText.text = "비겼습니다";

    }

    public void ReGameRequest()
    {
        _reGameText.gameObject.SetActive(true);
    }

    public void ReGameDisable()
    {
        _reGameText.gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
