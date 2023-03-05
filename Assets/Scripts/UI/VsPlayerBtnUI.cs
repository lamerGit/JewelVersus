using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VsPlayerBtnUI : MonoBehaviour
{
    Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickOnline);
    }


    void OnClickOnline()
    {
        //Managers.Network.ConnectToGame();
        C_EnterGame enterGamePacket = new C_EnterGame();
        enterGamePacket.Name = Managers.Network.LobbyPlayerInfo.Name;
        Managers.Network.Send(enterGamePacket);

        SceneManager.LoadScene((int)SceneEnum.VsPlayer);
        //if (Managers.Network.Token != "" )
        //{
        //    Managers.Network.ConnectToGame(Managers.Network.ServerInfo);
        //    SceneManager.LoadScene((int)SceneEnum.VsPlayer);
        //}else
        //{
        //    Debug.Log("로그인이 안되있습니다");
        //}
    }
}
