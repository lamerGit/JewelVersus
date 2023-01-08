using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VsAILoadingScene : MonoBehaviour
{
    string[] loadingText = new string[5] { "Loading", "Loading.", "Loading..", "Loading...", "Loading...." };

    WaitForSeconds waidSecond = new WaitForSeconds(0.2f);

    bool loadCompleted = false;

    TextMeshProUGUI loadingTextUI;

    float loadRatio = 0.0f;


    AsyncOperation async;

    private void Awake()
    {
        loadingTextUI= GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(LoadingTextProgress());
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadingTextProgress()
    {
        int count = 0;
        while(!loadCompleted)
        {

            loadingTextUI.text= loadingText[count];

            yield return waidSecond;

            count++;
            count %= 5;
            
        }
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync((int)SceneEnum.VsAI);
        async.allowSceneActivation= false;


        while(loadRatio<1.0f)
        {
            loadRatio = async.progress + 0.1f;

            yield return null;
        }

        

        yield return new WaitForSeconds(1.0f);
        loadCompleted = true;
        async.allowSceneActivation = true;

    }

}
