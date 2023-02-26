using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsPlayerGameTimer : MonoBehaviour
{
    Slider slider;
    
    public int Id { get; set; }

    public float SliderValue
    {
        get { return slider.value; }
        set
        {
            slider.value = value;
            if (slider.value <= 0)
            {
                GameManager.Instance.VsMyPlayerBlockManager.GameStart = false;
            }
        }


    }

    private void Awake()
    {
        slider = GetComponent<Slider>();

    }

}
