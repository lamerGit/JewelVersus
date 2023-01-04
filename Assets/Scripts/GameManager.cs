
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class GameManager : Singleton<GameManager>
{
    //게임매니저

    public static int BOARDX = 7; //블록 x값
    public static int BOARDY = 12; //블록 y값

    BlockManager blockManager = null;

    public BlockManager BlockManager
    {
        get { return blockManager; }
        set { blockManager = value; }
    }


}
