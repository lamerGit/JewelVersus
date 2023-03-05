using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    TextMeshProUGUI _winnerCount;
    TextMeshProUGUI _loseCount;
    TextMeshProUGUI _maxScore;
    TextMeshProUGUI _level;


    private void Awake()
    {
        _winnerCount=transform.Find("WinnerCount").GetComponent<TextMeshProUGUI>();
        _loseCount = transform.Find("LoseCount").GetComponent<TextMeshProUGUI>();
        _maxScore = transform.Find("MaxScore").GetComponent<TextMeshProUGUI>();
        _level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _winnerCount.text = $"Win : {Managers.Network.MyStatInfo.WinerCount}";
        _loseCount.text = $"Lose : {Managers.Network.MyStatInfo.LoseCount}";
        _maxScore.text = $"MaxScore : {Managers.Network.MyStatInfo.MaxScore}";
        _level.text = $"Level : {Managers.Network.MyStatInfo.Level}";

        GameManager.Instance.InfoUI = this;
    }

    public void RefreshInfo()
    {
        _winnerCount.text = $"Win : {Managers.Network.MyStatInfo.WinerCount}";
        _loseCount.text = $"Lose : {Managers.Network.MyStatInfo.LoseCount}";
        _maxScore.text = $"MaxScore : {Managers.Network.MyStatInfo.MaxScore}";
        _level.text = $"Level : {Managers.Network.MyStatInfo.Level}";
    }
}
