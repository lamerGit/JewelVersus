using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Block : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    //블럭의 스크립트

    public int indexX = -1; //블럭의 포지션X
    public int indexY = -1; //블럭의 포지션Y

    BlockType blockType = BlockType.None; //블럭의 타입
    Image image; //블럭의 이미지

    bool selected = false; //블럭이 선택됬는지 확인용 변수
    public bool isLive = false; // 블럭이 살아있는지 확인용 변수

    //블럭이 짝수일때랑 홀수일때 선택이 달라지기 때문에 
    //짝수와 홀수 검사용 변수를 따로 만든다. 위,아래,대각선왼쪽 오른쪽
    
    int[] dirEvenX = new int[6] { 1, 0, -1, 1, 0, -1 };
    int[] dirEvenY = new int[6] { 0, -1, 0, 1, 1, 1 };
    int[] dirOddX = new int[6] { 1, 0, -1, 1, 0, -1 };
    int[] dirOddY = new int[6] { -1, -1, -1, 0, 1, 0 };

    RectTransform rect; //UI로 만들었기 때문에 RectTransform을 사용

    public RectTransform[] lineGruop;

    int lineDir = -1;

    /// <summary>
    /// RectTransform 외부 참조용 프로퍼티
    /// </summary>
    public RectTransform Rect
    {
        get { return rect; }
    }

    /// <summary>
    /// 선택되었을때 해야할 행동을 정해두기위한 프로퍼티
    /// </summary>
    bool Selected
    {
        get { return selected; }
        set { selected = value;
        
            //선택되면 이미지를 흐리게 만든다.
            if(selected)
            {
                Color tempColor = image.color;
                tempColor.a = 0.5f;
                image.color = tempColor;


            }else
            {
                //선택해제시 이미지 원상복귀
                Color tempColor = image.color;
                tempColor.a = 1.0f;
                image.color = tempColor;
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
            if(blockType == BlockType.Red)
            {
                image.color= Color.red;
            }else if(blockType==BlockType.Green)
            {
                image.color = Color.green;
            }else if(blockType==BlockType.Blue)
            {
                image.color = Color.blue;
            }else if(blockType==BlockType.Black)
            {
                image.color=Color.black;
            }else if(blockType ==BlockType.White)
            {
                image.color = Color.white;
            }else if(blockType==BlockType.Pupple)
            {
                image.color = new Color(1, 0, 1, 1);
            }


        }
    }

    void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        lineGruop = GetComponentsInChildren<RectTransform>();

        for(int i=1; i<lineGruop.Length; i++)
        {
            lineGruop[i].SetAsLastSibling();
            lineGruop[i].gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// 블록을 클릭시 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //선택이 안되고 살아있을때만
        if (!Selected && isLive && GameManager.Instance.BlockManager.GameStart)
        {
            //선택 true로만들고
            //게임매니저에게 현재 어떤블록을 선택했는지 할당해주고
            //선택된 블록을 모아두는 queue에 넣는다.
            Selected = true;
            GameManager.Instance.BlockManager.ThisBlock = this;
            GameManager.Instance.BlockManager.Blocks.Enqueue(this);
        }


    }


    /// <summary>
    /// 블록위에 마우스가 올라왔을때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //게임매니저에 현재 블록이 선택되있으면
        if (GameManager.Instance.BlockManager.ThisBlock != null)
        {
            //자신이 선택상태가 아니고 살아있으면
            if (!Selected && isLive)
            {
                //게임매니저에 선택된 블록을 모아두는 queue가 0보다 클때
                if (GameManager.Instance.BlockManager.Blocks.Count > 0)
                {
                    //현재 블록이 연결된 블록인지 확인하는 함수가 참일때
                    if (CheckNextBlock())
                    {
                        //현재 블록과 타입이 같다면
                        if (GameManager.Instance.BlockManager.ThisBlock.blockType == BlockType)
                        {
                            //선택되었다고 표시
                            Selected = true;

                            GameManager.Instance.BlockManager.ThisBlock.LineEnable(lineDir);

                            //현재 블록을 자신으로
                            GameManager.Instance.BlockManager.ThisBlock = this;
                            //queue에 할당
                            GameManager.Instance.BlockManager.Blocks.Enqueue(this);
                        }
                    }
                }
            }
        }
        
    }

    /// <summary>
    /// 제작중....
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// 블록을 제거하는 함수
    /// 이동을 해야하기때문에 SetActive를 사용하지 않고
    /// isLive를 false하는 동시에 알파값을 0으로 만든다.
    /// </summary>
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

    /// <summary>
    /// 블록 활성함수 활성화 하면서 랜덤한 타입을 준다.
    /// </summary>
    /// <param name="x">활성할 블록의 x값</param>
    /// <param name="y">활성할 블록의 y값</param>
    public void BlockEnable(int x,int y)
    {
        isLive = true;
        Selected = false;
        indexX = x;
        indexY = y;

        Color tempColor = image.color;
        tempColor.a = 1.0f;
        image.color = tempColor;
        int r = Random.Range(1, 5);
        BlockType = (BlockType)r;
    }

    /// <summary>
    /// 선택을 해제하는 함수
    /// </summary>
    public void SelectedFalse()
    {
        Selected = false;
        for (int i = 1; i < lineGruop.Length; i++)
        {
            lineGruop[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 자신이 현재블록주변에 있는지 확인하는 변수
    /// </summary>
    /// <returns>현재블록주변에 있으면 true 아니면 false</returns>
    bool CheckNextBlock()
    {
        bool result = false;

        if (GameManager.Instance.BlockManager.ThisBlock.indexX % 2 == 0)
        {

            for (int i = 0; i < 6; i++)
            {
                int thisX = GameManager.Instance.BlockManager.ThisBlock.indexX + dirEvenX[i];
                int thisY = GameManager.Instance.BlockManager.ThisBlock.indexY + dirEvenY[i];

                if (indexX == thisX && indexY == thisY)
                {
                    result = true;
                    lineDir = i;
                    break;
                }

            }
        }else
        {
            for (int i = 0; i < 6; i++)
            {
                int thisX = GameManager.Instance.BlockManager.ThisBlock.indexX + dirOddX[i];
                int thisY = GameManager.Instance.BlockManager.ThisBlock.indexY + dirOddY[i];

                if (indexX == thisX && indexY == thisY)
                {
                    result = true;
                    lineDir = i;
                    break;
                }

            }
        }


        return result;
    }

    /// <summary>
    /// 블록정보 변경용 함수
    /// 사라지는 블록용 위치를 바로변경해야됨
    /// </summary>
    /// <param name="x">변경할 x값</param>
    /// <param name="y">변경할 y값</param>
    /// <param name="anchorPostion">변경할 위치</param>
    public void ValueChange(int x,int y,Vector2 anchorPostion)
    {
        indexX = x;
        indexY = y;
        rect.anchoredPosition = anchorPostion;
        
       
    } 

    /// <summary>
    /// 블록정보 변경용 함수
    /// 사라지지않고 위에 남는 블록용 위치가 천천히 변화해야함으로
    /// 위치는 따로변경해준다.
    /// </summary>
    /// <param name="x">변경할 x값</param>
    /// <param name="y">변경할 y값</param>
    public void ValueChange(int x, int y)
    {
        indexX = x;
        indexY = y;
        
    }

    public void LineEnable(int i)
    {
        lineGruop[i + 1].gameObject.SetActive(true);
    }


}
