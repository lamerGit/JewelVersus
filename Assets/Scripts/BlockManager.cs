using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class BlockManager : MonoBehaviour
{
    PlayerInput inputActions;

    //플레이어 변수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    bool isClick = false; // 클릭중인지 확인할 변수
    Queue<Block> blocks = new Queue<Block>(); // 선택한 블록을 담아두는 변수
    Block thisBlock = null; //현재 선택중인 변수
    Block[,] allBlocks = new Block[GameManager.BOARDX,GameManager.BOARDY]; //모든블록을 담아두는 변수
    int[] indexCheck = new int[7] { 0, 0, 0, 0, 0, 0, 0 }; //몇번째 indexX에 선택한 블록 갯수를 기록할 변수

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //적 변수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    Queue<EnemyBlock> enemyBlocks = new Queue<EnemyBlock>();
    EnemyBlock thisEnmeyBlock = null;
    EnemyBlock[,] enemyAllBlocks = new EnemyBlock[GameManager.BOARDX, GameManager.BOARDY];
    int[] enemyIndexCheck = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

    WaitForSeconds aiSpeed;

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    bool gameStart = false;


    float gameGauge = 0.5f;

    RectTransform vsGaugeRect;

    public GameObject bullet;

    GameObject canvas;

    Queue<Bullet> bullets = new Queue<Bullet>();

    RectTransform enemyImageRect;

    GameSetUI gameSetUI;

    //체크용 변수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    //플레이어와 AI가 3개이상 블록을 이을수 없는 것을 확인할 변수
    Queue<EnemyBlock> enemyCheckBlocks=new Queue<EnemyBlock>();
    EnemyBlock thisCheckEnemyBlock= null;

    public Queue<EnemyBlock> EnemyCheckBlocks
    {
        get { return enemyCheckBlocks; }
    }

    public EnemyBlock ThisCheckEnemyBlock
    {
        get { return thisCheckEnemyBlock; }
        set { thisCheckEnemyBlock = value; }
    }


    Queue<Block> checkBlocks=new Queue<Block>();
    Block thisCheckBlock= null;

    public Queue<Block> CheckBlocks
    {
        get { return checkBlocks; }
    }

    public Block ThisCheckBlock
    {
        get { return thisCheckBlock; }
        set { thisCheckBlock = value; }
    }



    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public Queue<Bullet> Bullets
    {
        get { return bullets; }
    }

    public RectTransform VsGaugeRect
    {
        get { return vsGaugeRect; }
    }

    public float GameGauge
    {
        get { return gameGauge; }
        set
        {
            gameGauge = Mathf.Clamp( value,-0.1f,1.1f);
            OnChangeGauge?.Invoke(gameGauge);
            if (gameGauge < 0.0f && GameStart)
            {
                GameStart = false;  
            }
            else if (gameGauge > 1.0f && GameStart)
            {
                GameStart = false;
            }

        }
    }

    public Action<float> OnChangeGauge;

    public bool GameStart
    {
        get { return gameStart; }
        set
        {
            gameStart = value;
            if (gameStart)
            {
                StartCoroutine(AutoEnemy());
                if (!GameBoardCheck())
                {

                    BoardReset();

                }

                if (!GameEnemyBoardCheck())
                {
                    EnemyBoardReset();

                }



                OnStartGame?.Invoke();
            }
            else
            {
                StopCoroutine(AutoEnemy());
                GameSet();
                if(gameGauge<0.5f)
                {
                    //Debug.Log("적 승리");
                    gameSetUI.Open(WinnerEnum.AI);


                }else if(gameGauge>0.5f)
                {
                    //Debug.Log("플레이어 승리");
                    gameSetUI.Open(WinnerEnum.Player);
                }
                else
                {
                    gameSetUI.Open(WinnerEnum.None);
                    //Debug.Log("무승부?");
                }
            }



        }
    }

    public Action OnStartGame;


    public EnemyBlock[,] EnemyAllBlocks
    {
        get { return enemyAllBlocks; }
        set { enemyAllBlocks = value; }
    }

    public EnemyBlock ThisEnmeyBlock
    {
        get { return thisEnmeyBlock; }
        set { thisEnmeyBlock = value; }
    }

    public Queue<EnemyBlock> EnemyBlocks
    {
        get { return enemyBlocks; }
    }


    bool IsClick
    {
        get { return isClick; }
        set
        {
            isClick = value;

        }
    }

    /// <summary>
    /// 모든 블록 프로퍼티
    /// </summary>
    public Block[,] AllBlocks
    {
        get { return allBlocks; }
        set { allBlocks = value; }
    }

    /// <summary>
    /// 현재 블록 프로퍼티
    /// </summary>
    public Block ThisBlock
    {
        get { return thisBlock; }
        set
        {
            thisBlock = value;

        }
    }

    /// <summary>
    /// 현재 선택중인 블록 프로퍼티
    /// </summary>
    public Queue<Block> Blocks
    {
        get { return blocks; }
    }



    private void Awake()
    {
        GameManager.Instance.BlockManager = this;
        inputActions = new();

        
        canvas = GameObject.Find("Canvas");

        for(int i=0; i<60; i++)
        {
            GameObject temp = Instantiate(bullet,canvas.transform);
            bullets.Enqueue(temp.GetComponent<Bullet>());
            temp.SetActive(false);
        }

    }

    private void Start()
    {
        //StartCoroutine(AutoEnemy());
        vsGaugeRect = FindObjectOfType<VsGauge>().GetComponent<RectTransform>();
        enemyImageRect=FindObjectOfType<EnemyImage>().GetComponent<RectTransform>();

        gameSetUI = FindObjectOfType<GameSetUI>();
        gameSetUI.Close();


        gameGauge = 0.5f;
        aiSpeed = new WaitForSeconds(GameManager.levelSpeed[GameManager.Instance.AILevel]);
    }

    private  void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Touch.performed += OnTouch;
        inputActions.Player.Touch.canceled += OnTouch;

    }

    private void OnDisable()
    {
        inputActions.Player.Touch.canceled -= OnTouch;
        inputActions.Player.Touch.performed -= OnTouch;
        inputActions.Disable();
    }

    /// <summary>
    /// 터치 밑 클릭시 실행되는 함수
    /// </summary>
    /// <param name="obj"></param>
    private void OnTouch(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {

            //클릭했을때
            IsClick = true;
        }
        else if (obj.canceled)
        {
            //클릭해제했을때
            IsClick = false;
            ThisBlock = null; // 현재 선택중인 블록없음처리
            //선택했던 블록이 3개이상이라면
            if (blocks.Count > 2)
            {
                //GaugeChange(blocks.Count, true);

                Block tempBlock = null;
                int tempCount=blocks.Count;
                //선택했던 블록 전부 제거
                while (blocks.Count > 0)
                {
                    tempBlock = blocks.Dequeue();
                    tempBlock.BlockDisable();

                    //선택했던 블록의 라인++
                    indexCheck[tempBlock.indexX]++;
                }
                if(tempBlock!= null)
                {
                    if(bullets.Count>0)
                    {
                        Bullet tempBullet = bullets.Dequeue();
                        tempBullet.MoveBullet(tempBlock.Rect.anchoredPosition,true,tempCount);
                    }

                }

                
                //라인 확인하면서 Check함수를 통해 교체함
                for (int i = 0; i < 7; i++)
                {
                    if (indexCheck[i] > 0)
                    {
                        Check(i, indexCheck[i]);
                    }
                    indexCheck[i] = 0;
                }

                if (!GameBoardCheck())
                {

                    BoardReset();
                    //Debug.Log("3개이상 이어지는 블록없음");
                }


            }
            else
            {
                //선택했던 블록이 3개이상이 아니라면 블록선택상태 해제
                while (blocks.Count > 0)
                {
                    Block tempBlock = blocks.Dequeue();

                    tempBlock.SelectedFalse();
                }
            }
        }
    }

    /// <summary>
    /// 사라진 블록수만큼 블록을 생성하고 ChangeBlock을 통해 블록위치를 변경
    /// </summary>
    /// <param name="indexX">현재 블록의 라인</param>
    /// <param name="blockCount">사라진 블록의 갯수</param>
    void Check(int indexX, int blockCount)
    {
        //절반의 블록은 생성용블록이다
        //그래서 (BOARDY/2-1)-i를 하면서 차례대로 블록생성
        for (int i = 0; i < blockCount; i++)
        {
            AllBlocks[indexX, (GameManager.BOARDY / 2 - 1) - i].BlockEnable(indexX, ((GameManager.BOARDY / 2 - 1) - i) - blockCount);
        }


        int emptyCount = 0; // 사라진 블록 갯수 카운트용
        for (int i = GameManager.BOARDY - 1; i >= 0; i--)
        {
            //블록이 살아있지않으면
            //emptyCount++해주고 continue를 해서 아래 코드가 실행되지 않게함
            if (!AllBlocks[indexX, i].isLive)
            {
                emptyCount++;
                continue;

            }
            //emptyCount만큼 블록이동
            if (emptyCount > 0)
            {
                ChangeBlock(indexX, i, i + emptyCount);


            }
        }


    }

    void EnemyCheck(int indexX, int blockCount)
    {
        //절반의 블록은 생성용블록이다
        //그래서 (BOARDY/2-1)-i를 하면서 차례대로 블록생성
        for (int i = 0; i < blockCount; i++)
        {
            EnemyAllBlocks[indexX, (GameManager.BOARDY / 2 - 1) - i].BlockEnable(indexX, ((GameManager.BOARDY / 2 - 1) - i) - blockCount);
        }


        int emptyCount = 0; // 사라진 블록 갯수 카운트용
        for (int i = GameManager.BOARDY - 1; i >= 0; i--)
        {
            //블록이 살아있지않으면
            //emptyCount++해주고 continue를 해서 아래 코드가 실행되지 않게함
            if (!EnemyAllBlocks[indexX, i].isLive)
            {
                emptyCount++;
                continue;

            }
            //emptyCount만큼 블록이동
            if (emptyCount > 0)
            {
                ChangeEnemyBlock(indexX, i, i + emptyCount);


            }
        }


    }




    /// <summary>
    /// 블록의 정보를 바꿔주는 함수
    /// </summary>
    /// <param name="x">블록의 라인</param>
    /// <param name="index1">이동할 블록</param>
    /// <param name="index2">없어질 블록</param>
    void ChangeBlock(int x, int index1, int index2)
    {
        //먼저 없어질 블록 정보를 따로 저장
        int tempx = AllBlocks[x, index2].indexX;
        int tempy = AllBlocks[x, index2].indexY;
        Vector2 tmpPos = AllBlocks[x, index2].Rect.anchoredPosition;
        Block tempBock = AllBlocks[x, index2];

        //먼저 없어질 블록정보를 index1정보로 바꾼다.
        AllBlocks[x, index2].ValueChange(AllBlocks[x, index1].indexX, AllBlocks[x, index1].indexY, AllBlocks[x, index1].Rect.anchoredPosition);
        //배열 정보도 바꾼다
        AllBlocks[x, index2] = AllBlocks[x, index1];

        //index1블록 위치변경
        MoveBlock(AllBlocks[x, index1], tmpPos);
        //index1블록 정보변경
        AllBlocks[x, index1].ValueChange(tempx, tempy);
        //배열 정보 바꿈
        AllBlocks[x, index1] = tempBock;




    }

    void ChangeEnemyBlock(int x, int index1, int index2)
    {
        //먼저 없어질 블록 정보를 따로 저장
        int tempx = enemyAllBlocks[x, index2].indexX;
        int tempy = enemyAllBlocks[x, index2].indexY;
        Vector2 tmpPos = enemyAllBlocks[x, index2].Rect.anchoredPosition;
        EnemyBlock tempBock = enemyAllBlocks[x, index2];

        //먼저 없어질 블록정보를 index1정보로 바꾼다.
        enemyAllBlocks[x, index2].ValueChange(enemyAllBlocks[x, index1].indexX, enemyAllBlocks[x, index1].indexY, enemyAllBlocks[x, index1].Rect.anchoredPosition);
        //배열 정보도 바꾼다
        enemyAllBlocks[x, index2] = enemyAllBlocks[x, index1];

        //index1블록 위치변경
        MoveEnemyBlock(enemyAllBlocks[x, index1], tmpPos);
        //index1블록 정보변경
        enemyAllBlocks[x, index1].ValueChange(tempx, tempy);
        //배열 정보 바꿈
        enemyAllBlocks[x, index1] = tempBock;




    }

    /// <summary>
    /// 블록이동함수 DOTween을 사용해서 완벽하게 이동한다.
    /// </summary>
    /// <param name="block">이동할블록</param>
    /// <param name="pos">이동할위치</param>
    void MoveBlock(Block block, Vector2 pos)
    {
        float duration = 0.0f;
        float speed = 100.0f;

        Vector2 prevPos = block.Rect.anchoredPosition;

        duration = Vector2.Distance(prevPos, pos) / speed;

        
        DOTween.To(() => block.Rect.anchoredPosition, x => block.Rect.anchoredPosition = x, pos, duration * Time.fixedDeltaTime);

    }


    void MoveEnemyBlock(EnemyBlock block, Vector2 pos)
    {
        float duration = 0.0f;
        float speed = 1000.0f;

        Vector2 prevPos = block.Rect.anchoredPosition;

        duration = Vector2.Distance(prevPos, pos) / speed;

        
        
        DOTween.To(() => block.Rect.anchoredPosition, x => block.Rect.anchoredPosition = x, pos, duration * Time.fixedDeltaTime);
        
        
    }

    IEnumerator AutoEnemy()
    {
        while (GameStart)
        {
            if (thisEnmeyBlock == null)
            {

                int enemyIndexX = UnityEngine.Random.Range(0, GameManager.BOARDX);
                int enemyIndexY = UnityEngine.Random.Range(GameManager.BOARDY / 2, GameManager.BOARDY);

                EnemyAllBlocks[enemyIndexX, enemyIndexY].FirstEnemyBlockSelect();
            }
            else
            {
                if (!thisEnmeyBlock.CheckEnemyNextBlock())
                {
                    if (enemyBlocks.Count > 2)
                    {
                        //GaugeChange(enemyBlocks.Count, false);


                        EnemyBlock tempBlock = null;
                        int tempCount=enemyBlocks.Count;

                        while (enemyBlocks.Count > 0)
                        {
                            tempBlock = enemyBlocks.Dequeue();
                            tempBlock.BlockDisable();
                            enemyIndexCheck[tempBlock.indexX]++;
                        }

                        if(tempBlock != null)
                        {
                            if(bullets.Count>0)
                            {
                                Bullet tempBullet = bullets.Dequeue();
                                tempBullet.MoveBullet(enemyImageRect.anchoredPosition, false,tempCount);
                            }
                        }



                        for (int i = 0; i < 7; i++)
                        {
                            if (enemyIndexCheck[i] > 0)
                            {
                                EnemyCheck(i, enemyIndexCheck[i]);
                            }
                            enemyIndexCheck[i] = 0;
                        }

                        thisEnmeyBlock = null;

                        if (!GameEnemyBoardCheck())
                        {
                            EnemyBoardReset();
                            //Debug.Log("3개이상 이어지는 블록없음");
                        }
                    }
                    else
                    {
                        //선택했던 블록이 3개이상이 아니라면 블록선택상태 해제
                        while (enemyBlocks.Count > 0)
                        {
                            EnemyBlock tempBlock = enemyBlocks.Dequeue();

                            tempBlock.SelectedFalse();
                        }
                        thisEnmeyBlock = null;
                    }
                }
            }


            yield return aiSpeed; 
        }
    }


    /// <summary>
    /// 3개 이상 열결될 블록이 있는지 확인하는 변수
    /// </summary>
    bool GameEnemyBoardCheck()
    {
        bool result=false;

        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = GameManager.BOARDY / 2-1; j <GameManager.BOARDY ; j++)
            {
                thisCheckEnemyBlock = EnemyAllBlocks[i, j];
                while(thisCheckEnemyBlock!=null)
                {
                    if (!thisCheckEnemyBlock.BoardCheckEnemyBlock())
                    {
                        if (enemyCheckBlocks.Count > 2)
                        {

                            while (enemyCheckBlocks.Count > 0)
                            {
                                EnemyBlock tempBlock = enemyCheckBlocks.Dequeue();

                                tempBlock.CheckedFalse();
                            }
                            result= true;

                            thisCheckEnemyBlock = null;
                        }
                        else
                        {
                            //선택했던 블록이 3개이상이 아니라면 블록선택상태 해제
                            while (enemyCheckBlocks.Count > 0)
                            {
                                EnemyBlock tempBlock = enemyCheckBlocks.Dequeue();

                                tempBlock.CheckedFalse();
                            }
                            thisCheckEnemyBlock = null;
                        }
                    }
                }
                if(result)
                {
                    break;
                }

            }
            if(result)
            {
                break;
            }
        }

        return result;
    }

    void EnemyBoardReset()
    {
        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0 ; j < GameManager.BOARDY; j++)
            {
                EnemyAllBlocks[i, j].BlockDisable();
            }
        }

        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY; j++)
            {
                EnemyAllBlocks[i, j].BlockEnable(i,j);
            }
        }

        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY / 2; j++)
            {
                EnemyAllBlocks[i, j].BlockDisable();

            }
        }
        if (!GameEnemyBoardCheck())
        {
            EnemyBoardReset();
            //Debug.Log("3개이상 이어지는 블록없음");
        }

    }


    bool GameBoardCheck()
    {
        bool result = false;

        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = GameManager.BOARDY / 2 - 1; j < GameManager.BOARDY; j++)
            {
                thisCheckBlock = AllBlocks[i, j];
                while (thisCheckBlock != null)
                {
                    if (!thisCheckBlock.BoardCheckBlock())
                    {
                        if (CheckBlocks.Count > 2)
                        {

                            while (CheckBlocks.Count > 0)
                            {
                                Block tempBlock = CheckBlocks.Dequeue();

                                tempBlock.CheckedFalse();
                            }
                            result = true;

                            thisCheckBlock = null;
                        }
                        else
                        {
                            //선택했던 블록이 3개이상이 아니라면 블록선택상태 해제
                            while (CheckBlocks.Count > 0)
                            {
                                Block tempBlock = CheckBlocks.Dequeue();

                                tempBlock.CheckedFalse();
                            }
                            thisCheckBlock = null;
                        }
                    }
                }
                if (result)
                {
                    break;
                }

            }
            if (result)
            {
                break;
            }
        }

        return result;
    }

    void BoardReset()
    {
        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY; j++)
            {
                AllBlocks[i, j].BlockDisable();
            }
        }

       
        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY; j++)
            {
                AllBlocks[i, j].BlockEnable(i,j);
            }
        }

        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY / 2; j++)
            {
                AllBlocks[i, j].BlockDisable();
            }
        }

        if (!GameBoardCheck())
        {

            BoardReset();
            //Debug.Log("3개이상 이어지는 블록없음");
        }


    }

    void GameSet()
    {
        while (blocks.Count > 0)
        {
            Block tempBlock = blocks.Dequeue();

            tempBlock.SelectedFalse();
        }
        thisBlock = null;

        while (enemyBlocks.Count > 0)
        {
            EnemyBlock tempBlock = enemyBlocks.Dequeue();

            tempBlock.SelectedFalse();
        }
        thisEnmeyBlock = null;
    }
}
