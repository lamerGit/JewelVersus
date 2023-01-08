using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public enum Achievements
    {
        Level1,
        Level5,
        Level10,
        Level15,
        Level20
    }

    class AchievementsComparer : IEqualityComparer<Achievements>
    {
        public bool Equals(Achievements a,Achievements b)
        {
            return a == b;
        }

        public int GetHashCode(Achievements obj)
        {
            return ((int)obj).GetHashCode();
        }
    }

    static AchievementManager _instance;

    public static AchievementManager Instance
    {
        get
        {
            if(_instance==null)
            {
                GameObject obj = new GameObject("AchievementManager");
                _instance= obj.AddComponent<AchievementManager>();
                DontDestroyOnLoad(obj);
            }

            return _instance;
        }
    }

    Dictionary<Achievements,bool> _dicAchievementUnlock=new Dictionary<Achievements,bool>(new AchievementsComparer());

    public Dictionary<Achievements,bool> DicAchievement
    {
        get { return _dicAchievementUnlock; }
    }

    public void OnNotify(Achievements achv,bool clearLevel=false)
    {
        switch(achv)
        {

            case Achievements.Level1:
                UnlockReachLevel1(clearLevel);
                break;
            case Achievements.Level5:
                UnlockReachLevel5(clearLevel);
                break;
            case Achievements.Level10:
                UnlockReachLevel10(clearLevel);
                break;
            case Achievements.Level15:
                UnlockReachLevel15(clearLevel);
                break;
            case Achievements.Level20:
                UnlockReachLevel20(clearLevel);
                break;

        }
    }

    public AchievementManager()
    {
        foreach(Achievements achv in Enum.GetValues(typeof(Achievements)))
        {
            _dicAchievementUnlock[achv] = false;
        }
    }

    private void UnlockReachLevel1(bool clearLevel)
    {
        if (_dicAchievementUnlock[Achievements.Level1])
            return;

        if (clearLevel)
        {
            Debug.Log("1레벨 클리어");
            _dicAchievementUnlock[Achievements.Level1] = true;
        }
    }
    private void UnlockReachLevel5(bool clearLevel)
    {
        if (_dicAchievementUnlock[Achievements.Level5])
            return;

        if (clearLevel)
        {
            Debug.Log("5레벨 클리어");
            _dicAchievementUnlock[Achievements.Level5] = true;
        }
    }

    private void UnlockReachLevel10(bool clearLevel)
    {
        if (_dicAchievementUnlock[Achievements.Level10])
            return;

        if (clearLevel)
        {
            Debug.Log("10레벨 클리어");
            _dicAchievementUnlock[Achievements.Level10] = true;
        }
    }

    private void UnlockReachLevel15(bool clearLevel)
    {
        if (_dicAchievementUnlock[Achievements.Level15])
            return;

        if (clearLevel)
        {
            Debug.Log("15레벨 클리어");
            _dicAchievementUnlock[Achievements.Level15] = true;
        }
    }

    private void UnlockReachLevel20(bool clearLevel)
    {
        if (_dicAchievementUnlock[Achievements.Level20])
            return;

        if (clearLevel)
        {
            Debug.Log("20레벨 클리어");
            _dicAchievementUnlock[Achievements.Level20] = true;
        }
    }
}
