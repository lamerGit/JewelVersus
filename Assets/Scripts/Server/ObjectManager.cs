using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    public VsMyPlayer MyPlayer { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public VsPlayerReadyGo ReadyGo { get; set; }
    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Add(ObjectInfo info, bool myPlayer = false)
    {
        GameObjectType objectType = GetObjectTypeById(info.ObjectId);

        switch (objectType)
        {
            case GameObjectType.None:
                break;
            case GameObjectType.Player:
                if (myPlayer)
                {
                    GameObject go = Managers.Resource.Instantiate("MyPlayer");
                    go.name = info.Name;
                    _objects.Add(info.ObjectId, go);

                    MyPlayer = go.GetComponent<VsMyPlayer>();
                    MyPlayer.Id = info.ObjectId;
                    MyPlayer.StateInfo = info.StateInfo;
                    MyPlayer.Stat = info.StatInfo;
                    
                    
                    if(MyPlayer.StateInfo.Team==PlayerTeam.Left)
                    {
                        MyPlayer.transform.position = new Vector3(-1.3f, 3.5f, 0.0f);
                    }else if(MyPlayer.StateInfo.Team==PlayerTeam.Right)
                    {
                        MyPlayer.transform.position = new Vector3(1.3f, 3.5f, 0.0f);
                    }

                }
                else
                {
                    GameObject go = Managers.Resource.Instantiate("Player");
                    go.name = info.Name;
                    _objects.Add(info.ObjectId, go);

                    VsPlayer pc = go.GetComponent<VsPlayer>();
                    pc.Id = info.ObjectId;
                    pc.StateInfo = info.StateInfo;
                    pc.Stat = info.StatInfo;

                    if (pc.StateInfo.Team == PlayerTeam.Left)
                    {
                        pc.transform.position = new Vector3(-1.3f, 3.5f, 0.0f);
                    }
                    else
                    {
                        pc.transform.position = new Vector3(1.3f, 3.5f, 0.0f);
                    }
                }
                break;
            case GameObjectType.Gauge:
                {
                    GameObject go = Managers.Resource.Instantiate("VsGauge",GameManager.Instance.MyCanvas.transform);
                    go.name = "Gauge";
                    _objects.Add(info.ObjectId, go);

                    VsPlayerGauge vs = go.GetComponent<VsPlayerGauge>();
                    vs.Id = info.ObjectId;
                    vs.SliderValue = info.GaugeInfo.GaugeValue;

                    GameManager.Instance.VsMyPlayerBlockManager.VsGaugeRect=vs.GetComponent<RectTransform>();

                }

                break;
            case GameObjectType.Timer:
                {
                    GameObject go = Managers.Resource.Instantiate("GameTimer", GameManager.Instance.MyCanvas.transform);
                    go.name = "Timer";
                    _objects.Add(info.ObjectId, go);

                    VsPlayerGameTimer vpgt= go.GetComponent<VsPlayerGameTimer>();
                    vpgt.Id = info.ObjectId;
                    vpgt.SliderValue = info.TimerInfo.Time;


                }
                break;
            case GameObjectType.Ready:
                {
                    GameObject go = Managers.Resource.Instantiate("Ready", GameManager.Instance.MyCanvas.transform);
                    go.name = "Ready";
                    _objects.Add(info.ObjectId, go);

                    VsPlayerReadyGo vprg= go.GetComponent<VsPlayerReadyGo>();
                    vprg.Id = info.ObjectId;
                    ReadyGo= vprg;

                }
                break;
        }


    }

    public void Remove(int id)
    {
        GameObject go = FindById(id);
        if (go == null)
            return;

        _objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public GameObject FindById(int id)
    {
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }


    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject obj in _objects.Values)
        {
            Managers.Resource.Destroy(obj);
        }


        _objects.Clear();

        MyPlayer = null;
    }


}
