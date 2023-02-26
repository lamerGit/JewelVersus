using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsPlayerGauge : MonoBehaviour
{
    Slider slider;

    public int Id { get; set; }

    public float SliderValue
    {
        get { return slider.value; }
        set { slider.value = value; }
    }

    private void Awake()
    {
        slider = GetComponent<Slider>();

    }

    
}
