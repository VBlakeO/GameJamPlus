using System;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;

public class PlayerCurrency : Singleton<PlayerCurrency>, IPersistent
{
    string _persistentPath = "/Currency";

    [SerializeField] Data _data = new Data();
    public Data data => _data;

    public int amount => data.amount;

    public Action<int> onAmountChanged;


    protected override void Awake()
    {
        base.Awake();

        ((IPersistent)this).Subscribe();
    }

    void IPersistent.Subscribe()
    {
        Persistent.Instance.onSave += ((IPersistent)this).SaveAsJson;
        Persistent.Instance.onLoad += ((IPersistent)this).LoadFromJson;
    }
    void IPersistent.Unsubscribe()
    {
        Persistent.Instance.onSave -= ((IPersistent)this).SaveAsJson;
        Persistent.Instance.onLoad -= ((IPersistent)this).LoadFromJson;
    }

    public void Add(int quantity)
    {
        data.amount += quantity;

        if (onAmountChanged != null)
            onAmountChanged.Invoke(data.amount);
    }
    public void Remove(int quantity)
    {
        data.amount -= quantity;

        if (onAmountChanged != null)
            onAmountChanged.Invoke(data.amount);
    }

    bool IPersistent.HasJsonSave(string persistentDataPath)
    {
        return File.Exists(persistentDataPath + _persistentPath);
    }
    void IPersistent.SaveAsJson(string persistentDataPath)
    {
        File.WriteAllText(persistentDataPath + _persistentPath, JsonConvert.SerializeObject(_data, Formatting.Indented));
    }
    void IPersistent.LoadFromJson(string persistentDataPath)
    {
        if (!((IPersistent)this).HasJsonSave(persistentDataPath))
        {
            ((IPersistent)this).SaveAsJson(persistentDataPath);
            return;
        }

        _data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(persistentDataPath + _persistentPath));

        if (onAmountChanged != null)
            onAmountChanged.Invoke(_data.amount);
    }


    [Serializable]
    public class Data
    {
        public int amount = 0;
    }
}