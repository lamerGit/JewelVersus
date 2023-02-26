using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VsPlayerBlockManager : MonoBehaviour
{
    VsPlayerBlock[,] allBlocks = new VsPlayerBlock[GameManager.BOARDX, GameManager.BOARDY]; //상대방 블록을 담아두는 변수

    VsPlayerBlock _beforBlock = null;

    public VsPlayerBlock BeforBlock
    {
        get { return _beforBlock; }
        set { _beforBlock = value; }
    }

    public VsPlayerBlock[,] AllBlocks
    {
        get { return allBlocks; }
        set { allBlocks = value; }
    }

    private void Awake()
    {
        GameManager.Instance.VsPlayerBlockManager = this;
        
    }

    public void CheckLiveBlock()
    {
        for (int i = 0; i < GameManager.BOARDX; i++)
        {
            for (int j = 0; j < GameManager.BOARDY; j++)
            {
                if (!AllBlocks[i, j].isLive)
                {
                    AllBlocks[i, j].BlockDisable();
                }
            }
        }
    }
}
