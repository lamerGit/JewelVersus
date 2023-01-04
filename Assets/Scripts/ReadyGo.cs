using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadyGo : MonoBehaviour
{
    int count = 3;
    WaitForSeconds timer = new WaitForSeconds(1.0f);
    TextMeshProUGUI textGui;


    private void Awake()
    {
        textGui= GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(GameStartTimer());
        
    }

    IEnumerator GameStartTimer()
    {
        while(count>0)
        {
            count--;

            if(count==0)
            {
                textGui.text = "GO!";
                GameManager.Instance.BlockManager.GameStart = true;
            }


            yield return timer;
        }
        gameObject.SetActive(false);
        
    }
}
