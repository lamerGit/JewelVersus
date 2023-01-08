using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VsAIUI : MonoBehaviour
{
    Button aiStart;
    Button aiIncrease;
    Button aiDecrease;
    TextMeshProUGUI stageNumber;

    int thisStage = 1;

    int ThisStage
    {
        get { return thisStage; }
        set { thisStage = Mathf.Clamp( value,1,20);

            stageNumber.text = thisStage.ToString();

            if(thisStage==1)
            {
                aiDecrease.gameObject.SetActive(false);
            }else
            {
                aiDecrease.gameObject.SetActive(true);
            }

            if(thisStage==20)
            {
                aiIncrease.gameObject.SetActive(false);
            }else
            {
                aiIncrease.gameObject.SetActive(true);
            }

        }
    }

    private void Awake()
    {
        aiStart=transform.Find("AIStart").GetComponent<Button>();
        aiIncrease=transform.Find("AIIncrease").GetComponent<Button>();
        aiDecrease=transform.Find("AIDecrease").GetComponent<Button>();
        stageNumber=transform.Find("StageNumber").GetComponent<TextMeshProUGUI>();

        ThisStage = 1;
        stageNumber.text=thisStage.ToString();

        aiStart.onClick.AddListener(OnAiStart);
        aiIncrease.onClick.AddListener(OnInscrease);
        aiDecrease.onClick.AddListener(OnDecrease);
    }



    void OnInscrease()
    {
        ThisStage++;
        if (!GameManager.Instance.LevelClear[ThisStage-1])
        {
            aiIncrease.interactable= false;
        }
    }

    void OnDecrease()
    {
        ThisStage--;
        aiIncrease.interactable= true;
    }

    void OnAiStart()
    {
        GameManager.Instance.AILevel = thisStage-1;
        SceneManager.LoadScene((int)SceneEnum.VsAILoading);
    }

}
