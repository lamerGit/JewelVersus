using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    Slider slider;
    WaitForSeconds t = new WaitForSeconds(1.0f);

    private void Awake()
    {
        slider = GetComponent<Slider>();

    }

    private void Start()
    {
        GameManager.Instance.BlockManager.OnStartGame += StartTimer;
    }


    void StartTimer()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
       
        while(GameManager.Instance.BlockManager.GameStart)
        {
            slider.value -= 0.01f;

            if(slider.value <= 0 )
            {
                GameManager.Instance.BlockManager.GameStart= false; 
            }

            yield return t;
        }
    }

}
