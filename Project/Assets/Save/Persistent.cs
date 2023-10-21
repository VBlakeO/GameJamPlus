using System;
using UnityEngine;

public class Persistent : Singleton<Persistent>
{
    string _persistentDataPath;

    public Action<string> onSave; //string path
    public Action<string> onLoad; //string path

    protected override void Awake()
    {
        base.Awake();
        _persistentDataPath = Application.persistentDataPath + "/Persistent";
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
            onSave.Invoke(_persistentDataPath);
    }

    public void Load()
    {
        if (onLoad != null)
            onLoad.Invoke(_persistentDataPath);
    }
}