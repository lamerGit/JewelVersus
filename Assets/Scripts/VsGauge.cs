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
            slider.value = value;
        };
    }

}
