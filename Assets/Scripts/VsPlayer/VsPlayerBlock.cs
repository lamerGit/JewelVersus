using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VsPlayerBlock : MonoBehaviour
{
    public int indexX = -1; //블럭의 포지션X
    public int indexY = -1; //블럭의 포지션Y

    BlockType blockType = BlockType.Notthing; //블럭의 타입

    Image image; //블럭의 이미지

    bool selected = false; //블럭이 선택됬는지 확인용 변수
    public bool isLive = false; // 블럭이 살아있는지 확인용 변수

    public RectTransform[] lineGruop;

    //블럭이 짝수일때랑 홀수일때 선택이 달라지기 때문에 
    //짝수와 홀수 검사용 변수를 따로 만든다. 위,아래,대각선왼쪽 오른쪽

    int[] dirEvenX = new int[6] { 1, 0, -1, 1, 0, -1 };
    int[] dirEvenY = new int[6] { 0, -1, 0, 1, 1, 1 };
    int[] dirOddX = new int[6] { 1, 0, -1, 1, 0, -1 };
    int[] dirOddY = new int[6] { -1, -1, -1, 0, 1, 0 };

    /// <summary>
    /// 선택되었을때 해야할 행동을 정해두기위한 프로퍼티
    /// </summary>
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;

            //선택되면 이미지를 흐리게 만든다.
            if (selected)
            {
                Color tempColor = image.color;
                tempColor.a = 0.5f;
                image.color = tempColor;


            }
            else
            {
                if (isLive)
                {
                    //선택해제시 이미지 원상복귀
                    Color tempColor = image.color;
                    tempColor.a = 1.0f;
                    image.color = tempColor;

                    for (int i = 1; i < lineGruop.Length; i++)
                    {
                        lineGruop[i].gameObject.SetActive(false);
                    }
                }
            }

        }

    }


    /// <summary>
    /// 블록타입enum 외부참조용 변수
    /// </summary>
    public BlockType BlockType
    {
        get { return blockType; }
        set
        {
            blockType = value;

            //블록타입에 따라 블록색이 변한다.

            switch (blockType)
            {
                case BlockType.Notthing:
                    image.color = new Color(0, 0, 0, 0);
                    break;
                case BlockType.Red:
                    image.color = Color.red;
                    break;
                case BlockType.Green:
                    image.color = Color.green;
                    break;
                case BlockType.Blue:
                    image.color = Color.blue;
                    break;
                case BlockType.Pupple:
                    image.color = new Color(1, 0, 1, 1);
                    break;
                case BlockType.Black:
                    image.color = Color.black;
                    break;
                case BlockType.White:
                    image.color = Color.white;
                    break;
            }




        }
    }

    void Awake()
    {
        image = GetComponent<Image>();
        //rect = GetComponent<RectTransform>();
        lineGruop = GetComponentsInChildren<RectTransform>();

        for (int i = 1; i < lineGruop.Length; i++)
        {
            lineGruop[i].SetAsLastSibling();
            lineGruop[i].gameObject.SetActive(false);
        }

    }

    public void BlockDisable()
    {
        isLive = false;
        Color tempColor = image.color;
        tempColor.a = 0.0f;
        image.color = tempColor;
        for (int i = 1; i < lineGruop.Length; i++)
        {
            lineGruop[i].gameObject.SetActive(false);
        }
    }

    public void InfoChange(BlockInfo info)
    {
        indexX = info.X; 
        indexY=info.Y;
        Selected=info.Selected;
        isLive = info.IsLive;
        BlockType = info.Type;
    }
    public void LineEnable(int i)
    {
        lineGruop[i + 1].gameObject.SetActive(true);
    }

    /// <summary>
    /// 이전에 선택된 블록이 있으면 그 블록과 연결해주는 함수
    /// </summary>
    /// <returns></returns>
    public void CheckBeforBlock()
    {
        if (GameManager.Instance.VsPlayerBlockManager.BeforBlock == null)
            return;
        
        if (indexX % 2 == 0)
        {

            for (int i = 0; i < 6; i++)
            {
                int beforX = indexX + dirEvenX[i];
                int beforY = indexY + dirEvenY[i];

                if (beforX >= 0 && beforX < GameManager.BOARDX && beforY >= GameManager.BOARDY / 2 && beforY < GameManager.BOARDY)
                {
                    if (GameManager.Instance.VsPlayerBlockManager.AllBlocks[beforX, beforY].BlockType == blockType && GameManager.Instance.VsPlayerBlockManager.AllBlocks[beforX, beforY].Selected)
                    {
                        
                        LineEnable(i);
                        
                        

                        break;
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                int beforX = indexX + dirOddX[i];
                int beforY = indexY + dirOddY[i];

                if (beforX >= 0 && beforX < GameManager.BOARDX && beforY >= GameManager.BOARDY / 2 && beforY < GameManager.BOARDY)
                {
                    if (GameManager.Instance.VsPlayerBlockManager.AllBlocks[beforX, beforY].BlockType == blockType && GameManager.Instance.VsPlayerBlockManager.AllBlocks[beforX, beforY].Selected)
                    {
                       
                        LineEnable(i);
                        

                        break;
                    }
                }

            }
        }

    }

    public void ConnectLine(int infoX,int infoY)
    {
        if (indexX % 2 == 0)
        {

            for (int i = 0; i < 6; i++)
            {
                int beforX = indexX + dirEvenX[i];
                int beforY = indexY + dirEvenY[i];

                if (beforX >= 0 && beforX < GameManager.BOARDX && beforY >= GameManager.BOARDY / 2 && beforY < GameManager.BOARDY)
                {
                    if (GameManager.Instance.VsPlayerBlockManager.AllBlocks[infoX, infoY].BlockType == blockType && beforX ==infoX && beforY== infoY && GameManager.Instance.VsPlayerBlockManager.AllBlocks[infoX, infoY].Selected)
                    {

                        LineEnable(i);



                        break;
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                int beforX = indexX + dirOddX[i];
                int beforY = indexY + dirOddY[i];

                if (beforX >= 0 && beforX < GameManager.BOARDX && beforY >= GameManager.BOARDY / 2 && beforY < GameManager.BOARDY)
                {
                    if (GameManager.Instance.VsPlayerBlockManager.AllBlocks[infoX, infoY].BlockType == blockType && beforX == infoX && beforY == infoY && GameManager.Instance.VsPlayerBlockManager.AllBlocks[infoX, infoY].Selected)
                    {

                        LineEnable(i);


                        break;
                    }
                }

            }
        }

    }


}
