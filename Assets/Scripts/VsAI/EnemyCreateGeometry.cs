using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreateGeometry : MonoBehaviour
{
    //적 블록을 생성하는 클래스

    public GameObject enemyBlock; //생성할 블록

    int boardx = 7; //기본 x값 개수
    int boardy = 6; //기본 y값 개수

    //오브젝트간의 간격변수
    float intervalX = 200.0f; // 오브젝트의 RectTransform의 width값 
    float intervalY = -250.0f; // 오브젝트의 RectTransform의 height값인데 그대로하면 간격이 너무 좁아서 1/4값 더해줌


    void Start()
    {
        boardx = GameManager.BOARDX; //게임매니저에서 게임에서 쓸 블록갯수 받아옴
        boardy = GameManager.BOARDY;

        CreateCircle();//생성

        // 절반은 생성용이라서 절반은 제거처리
        for (int i = 0; i < boardx; i++)
        {
            for (int j = 0; j < boardy / 2; j++)
            {
                GameManager.Instance.BlockManager.EnemyAllBlocks[i, j].BlockDisable();
            }
        }
    }

    /// <summary>
    /// 블록생성함수
    /// </summary>
    void CreateCircle()
    {

        for (int i = 0; i < boardx; i++)
        {
            for (int j = 0; j < boardy; j++)
            {
                GameObject temp = Instantiate(enemyBlock, transform); //블록생성
                EnemyBlock tempblock = temp.GetComponent<EnemyBlock>();  //컴포넌트받고

                tempblock.indexX = i; //x값 할당
                tempblock.indexY = j; // y값 할당
                int r = Random.Range(1, GameManager.MAXBLOCK); // 블록타입 랜덤으로 뽑고
                tempblock.BlockType = (BlockType)r;
                GameManager.Instance.BlockManager.EnemyAllBlocks[i, j] = tempblock; // 게임매니저에 준다.
                tempblock.isLive = true;


                //짝수랑 홀수랑 다르게 배치해야함으로 구분해줌
                if (i % 2 == 1 && i != 0)
                {
                    Vector2 tempVector = new Vector2(intervalX * i, intervalY * j + 125.0f);
                    temp.GetComponent<RectTransform>().anchoredPosition = tempVector;


                }
                else
                {
                    Vector2 tempVector = new Vector2(intervalX * i, intervalY * j);
                    temp.GetComponent<RectTransform>().anchoredPosition = tempVector;


                }
            }
        }


    }


}
