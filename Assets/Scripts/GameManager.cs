
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //게임매니저

    public static int BOARDX = 7; //블록 x값
    public static int BOARDY = 12; //블록 y값

    public static int MAXBLOCK = 5; // 최대 생성되는 블록갯수

    BlockManager blockManager = null;
    VsMyPlayerBlockManager vsMyPlayerBlockManager = null;
    VsPlayerBlockManager vsPlayerBlockManager = null;

    public InfoUI InfoUI { get; set; } = null;

    public static float[] levelSpeed = new float[20] { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.09f, 0.08f, 0.07f, 0.06f, 0.05f, 0.04f, 0.03f, 0.02f, 0.01f, 0.009f };

    bool[] levelClear= new bool[20];

    int aiLevel=0;

   
    bool[] achievementEvent = new bool[5] {false,false,false,false,false };

    Canvas _mycanvas= null;

    public Canvas MyCanvas
    {
        get { return _mycanvas; }
    }

    public bool[] AchievementEvent
    {
        get { return achievementEvent; }
        set { achievementEvent = value; }
    }

    public bool[] LevelClear
    {
        get { return levelClear; }
        set { levelClear = value; }
    }

    public int AILevel
    {
        get { return aiLevel; }
        set { aiLevel = value; }
    }


    public BlockManager BlockManager
    {
        get { return blockManager; }
        set { blockManager = value; }
    }

    public VsMyPlayerBlockManager VsMyPlayerBlockManager
    {
        get { return vsMyPlayerBlockManager; }
        set { vsMyPlayerBlockManager = value; }
    }

    public VsPlayerBlockManager VsPlayerBlockManager
    {
        get { return vsPlayerBlockManager; }
        set { vsPlayerBlockManager = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        for(int i=0; i<levelClear.Length; i++)
        {
            levelClear[i] = false;
        }
        //levelClear[0]= true;
      
    }

    protected override void Initialize()
    {
        Scene scene = SceneManager.GetActiveScene();

        //Debug.Log(scene.buildIndex);
        if (scene.buildIndex == (int)SceneEnum.VsPlayer)
        {
            _mycanvas = GameObject.Find("MyCanvas").GetComponent<Canvas>();
            Managers.Clear();
        }

        
    }

    
    public void OpenLoginFail()
    {
        Canvas canvas= GameObject.Find("Canvas").GetComponent<Canvas>();
        Managers.Resource.Instantiate("LoginFailMessage",canvas.transform);
    }

   

    


}
