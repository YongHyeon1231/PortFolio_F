﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    MapManager _map = new MapManager();
    QuestManager _quest = new QuestManager();
    TalkManager _talk = new TalkManager();
    GameManager _game = new GameManager();

    public static MapManager Map { get { return Instance._map; } }
    public static QuestManager Quest { get { return Instance._quest; } }
    public static TalkManager Talk { get { return Instance._talk; } }
    public static GameManager Game { get { return Instance._game; } }
    #endregion

    #region Core
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Init();
        _game = Util.GetOrAddComponent<GameManager>(GameObject.Find("GameManager"));
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._pool.Init();
            s_instance._talk.Init();
            s_instance._quest.Init();
        }
    }

    public static void Clear()
    {
        Scene.Clear();
        Pool.Clear();
        UI.Clear();
    }
}

