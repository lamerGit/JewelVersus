using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect= GetComponent<RectTransform>();
    }


    public void MoveBullet(Vector2 pos,bool player,int damageCount)
    {
        gameObject.SetActive(true);

        float duration = 0.0f;
        float speed = 50.0f;


        if(player)
        {
            Vector2 temp = pos;

            temp.x -= 595.0f;
            temp.y += 1770.0f;

            rect.anchoredPosition = temp;
        }else
        {
            rect.anchoredPosition = pos;
        }

        RectTransform target = GameManager.Instance.BlockManager.VsGaugeRect;

        Vector2 targetVector = target.anchoredPosition;
        targetVector.y -= 50.0f;
        targetVector.x -= 50.0f;

        
        duration = Vector2.Distance(rect.anchoredPosition, targetVector) / speed;

        DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, targetVector, duration * Time.fixedDeltaTime).OnComplete(() => { GaugeChange(damageCount, player);
            BulletDisable();
        });
    }


    void BulletDisable()
    {

        GameManager.Instance.BlockManager.Bullets.Enqueue(this);

        gameObject.SetActive(false);
    }

    void GaugeChange(int count, bool player)
    {
        float attackType = 1.0f;

        if (!player)
        {
            attackType *= -1.0f;
        }


        for (int i = 1; i <= count; i++)
        {
            if (i < 4)
            {
                GameManager.Instance.BlockManager.GameGauge += 0.01f * attackType;
            }
            else if (i < 7)
            {
                GameManager.Instance.BlockManager.GameGauge += 0.02f * attackType;
            }
            else if (i < 10)
            {
                GameManager.Instance.BlockManager.GameGauge += 0.03f * attackType;
            }
            else
            {
                GameManager.Instance.BlockManager.GameGauge += 0.04f * attackType;
            }
            //Debug.Log(GameGauge);
        }


    }

}
