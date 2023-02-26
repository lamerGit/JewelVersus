using DG.Tweening;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsPlayerBullet : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    public void MoveBullet(Vector2 pos, bool player, int damageCount)
    {
        gameObject.SetActive(true);

        float duration = 0.0f;
        float speed = 50.0f;


        if (player)
        {
            Vector2 temp = pos;

            temp.x -= 595.0f;
            temp.y += 1770.0f;

            rect.anchoredPosition = temp;
        }
        else
        {
            rect.anchoredPosition = pos;
        }

        RectTransform target = GameManager.Instance.VsMyPlayerBlockManager.VsGaugeRect;

        if(target == null)
        {
            return;
        }

        Vector2 targetVector = target.anchoredPosition;
        targetVector.y -= 50.0f;
        targetVector.x -= 50.0f;


        duration = Vector2.Distance(rect.anchoredPosition, targetVector) / speed;

        DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, targetVector, duration * Time.fixedDeltaTime).OnComplete(() => {
            GaugeChange(damageCount);
            BulletDisable();
        });
    }


    void BulletDisable()
    {

        GameManager.Instance.VsMyPlayerBlockManager.Bullets.Enqueue(this);

        gameObject.SetActive(false);
    }

    void GaugeChange(int count)
    {
        float resultGauge = 0.0f;


        for (int i = 1; i <= count; i++)
        {
            if (i < 4)
            {
                resultGauge += 0.01f ;
            }
            else if (i < 7)
            {
                resultGauge += 0.02f ;
            }
            else if (i < 10)
            {
                resultGauge += 0.03f ;
            }
            else
            {
                resultGauge += 0.04f ;
            }
            //Debug.Log(GameGauge);
        }

        C_Gauge gauge = new C_Gauge() { Info = new GaugeInfo() };
        gauge.Info.GaugeValue = resultGauge;
        Managers.Network.Send(gauge);

    }

}
