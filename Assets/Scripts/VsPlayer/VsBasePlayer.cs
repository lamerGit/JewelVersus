using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsBasePlayer : MonoBehaviour
{
    public int Id { get; set; }

    StatInfo _stat = new StatInfo();

    StateInfo _stateInfo = new StateInfo();

    protected Animator _animator;
    protected SpriteRenderer _sprite;

    public virtual StatInfo Stat
    {
        get { return _stat; }
        set {
            if (_stat.Equals(value))
                return;

            _stat.Level = value.Level;
        
        }
    }

    /// <summary>
    /// 상태정보 프로퍼티
    /// </summary>
    public StateInfo StateInfo
    {
        get { return _stateInfo; }
        set {
            if (_stateInfo.Equals(value))
                return;

            State = value.State;
            Team= value.Team;
        
        
        }
    }

    public PlayerTeam Team
    {
        get { return StateInfo.Team; }
        set
        {
            if (StateInfo.Team == value)
                return;

            StateInfo.Team = value;
        }
    }
    /// <summary>
    /// 상태 프로퍼티
    /// </summary>
    public virtual CreatureState State
    {
        get { return StateInfo.State; }
        set
        {
            if (StateInfo.State == value)
                return;

            StateInfo.State = value;
            UpdateAnimation();
        }
    }


    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _animator= GetComponent<Animator>();
        _sprite= GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 상태에 따른 애니메이션 변경
    /// </summary>
    protected virtual void UpdateAnimation()
    {
        
    }


    
}
