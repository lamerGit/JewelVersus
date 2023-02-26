using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VsPlayerReadyGo : MonoBehaviour
{
    public int Id { get; set; }

    TextMeshProUGUI textGui;
    WaitForSeconds timer = new WaitForSeconds(1.0f);

    private void Awake()
    {
        textGui = GetComponent<TextMeshProUGUI>();
    }

    public void Go()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        textGui.text = "GO!";
        GameManager.Instance.VsMyPlayerBlockManager.GameStart = true;
        yield return timer;
        gameObject.SetActive(false);
    }

    
}
