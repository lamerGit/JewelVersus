using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    Image achieveImage;

    TextMeshProUGUI achieveTitle;
    TextMeshProUGUI achieveEx;

    Button closeButton;

    string level1title = "업적달성 : \n시작";
    string level5title = "업적달성 : \n이정도쯤이야";
    string level10title = "업적달성 : \n보석의 초보자";
    string level15title = "업적달성 : \n보석의 중급자";
    string level20title = "업적달성 : \n보석의 상급자";

    string level1ex = "Level1 AI 상대로 승리";
    string level5ex = "Level5 AI 상대로 승리";
    string level10ex = "Level10 AI 상대로 승리";
    string level15ex = "Level15 AI 상대로 승리";
    string level20ex = "Level20 AI 상대로 승리";


    private void Awake()
    {
        achieveImage = transform.Find("AchieveImage").GetComponent<Image>();
        achieveTitle=transform.Find("AchieveTitle").GetComponent<TextMeshProUGUI>();
        achieveEx = transform.Find("AchieveEx").GetComponent<TextMeshProUGUI>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();

        closeButton.onClick.AddListener(Close);

    }

    private void Start()
    {
        Close();
        Open();
    }

    void Open()
    {
        if (AchievementManager.Instance.DicAchievement[AchievementManager.Achievements.Level1] && !GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level1])
        {
            gameObject.SetActive(true);
            GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level1] = true;
            achieveTitle.text = level1title;
            achieveEx.text = level1ex;

        }

        if (AchievementManager.Instance.DicAchievement[AchievementManager.Achievements.Level5] && !GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level5])
        {
            gameObject.SetActive(true);
            GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level5] = true;
            achieveTitle.text = level5title;
            achieveEx.text = level5ex;

        }

        if (AchievementManager.Instance.DicAchievement[AchievementManager.Achievements.Level10] && !GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level10])
        {
            gameObject.SetActive(true);
            GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level10] = true;
            achieveTitle.text = level10title;
            achieveEx.text = level10ex;

        }

        if (AchievementManager.Instance.DicAchievement[AchievementManager.Achievements.Level15] && !GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level15])
        {
            gameObject.SetActive(true);
            GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level15] = true;
            achieveTitle.text = level15title;
            achieveEx.text = level15ex;

        }

        if (AchievementManager.Instance.DicAchievement[AchievementManager.Achievements.Level20] && !GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level20])
        {
            gameObject.SetActive(true);
            GameManager.Instance.AchievementEvent[(int)AchievementManager.Achievements.Level20] = true;
            achieveTitle.text = level20title;
            achieveEx.text = level20ex;

        }
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

}
