using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    TextMeshProUGUI _info;

    TMP_InputField _accountInput;


    Button _createButton;
    Button _loginButton;


    private void Awake()
    {
        _info=transform.Find("LoginInfo").GetComponent<TextMeshProUGUI>();
       


        GameObject fields = transform.Find("Fields").gameObject;
        _accountInput=fields.transform.Find("AccountName").GetComponent<TMP_InputField>();

        _createButton=transform.Find("CreateBtn").GetComponent<Button>();
        _loginButton = transform.Find("LoginBtn").GetComponent<Button>();

        _createButton.onClick.AddListener(OnGoogleLogout);
        _loginButton.onClick.AddListener(OnGoogleLoginBtn);

        _info.gameObject.SetActive(false);
        _createButton.gameObject.SetActive(false);
    }


    void OnGoogleLoginBtn()
    {
        //GPGSBinder.Inst.Login((sucess, localUser) =>
        //{
        //    if(sucess==true)
        //    {
        //        _info.text = $"{sucess},{localUser.userName},{localUser.id},{localUser.state},{localUser.underage}";

        //        LoginGoogleAccountPacketReq packet = new LoginGoogleAccountPacketReq()
        //        {
        //            Token = localUser.id
        //        };

        //        Managers.Web.SendPostRequest<LoginGoogleAccountPacketRes>("account/login/google", packet, (res) =>
        //        {
        //            Debug.Log(res);
        //        });
        //    }

        //});


        LoginGoogleAccountPacketReq packet = new LoginGoogleAccountPacketReq()
        {
            Token = _accountInput.text
        };

        Managers.Web.SendPostRequest<LoginGoogleAccountPacketRes>("account/login/google", packet, (res) =>
        {
            Debug.Log(res.LoginOk);

            if(res.LoginOk)
            {
                Managers.Network.Token = res.JwtAccessToken;
                Managers.Network.ConnectToGame();
                SceneManager.LoadScene((int)SceneEnum.Robby);
            }

        });
        Debug.Log(Managers.Network.Token);
        _accountInput.text = "";
    }

    void OnGoogleLogout()
    {
        //GPGSBinder.Inst.Logout();
    }
}
