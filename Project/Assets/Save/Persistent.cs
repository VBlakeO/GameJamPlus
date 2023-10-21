using System;
using UnityEngine;

public class Persistent : Singleton<Persistent>
{
    public string persistentDataPath { get; private set; }

    public Action<string> onSave; //string path
    public Action<string> onLoad; //string path

    protected override void Awake()
    {
        base.Awake();
        persistentDataPath = Application.persistentDataPath;
        Debug.Log(persistentDataPath);
    }

    protected override void OnApplicationQuit()
    {
        base.Awake();
        Save();
    }

    void Start()
    {
        Load();
    }

    public void Save()
    {
        if (onSave != null)
            onSave.Invoke(persistentDataPath);
    }

    public void Load()
    {
        if (onLoad != null)
            onLoad.Invoke(persistentDataPath);
    }
}