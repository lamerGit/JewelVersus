using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsGauge : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider= GetComponent<Slider>();
        
    }

    private void Start()
    {
        GameManager.Instance.BlockManager.OnChangeGauge += (value) =>
        {
            if (GameManager.Instance.BlockManager.GameStart)
            {
                slider.value = value;
            }
        };
    }

}
