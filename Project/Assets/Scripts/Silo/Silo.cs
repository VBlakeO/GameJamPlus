using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Silo : Singleton<Silo>, IPersistent
{
    string _persistentPath = "/Silo";

    public int maxQuantity => data.maxQuantity;
    public string id => data.id;
    public int quantity => data.quantity;

    
    [SerializeField] Data _data = new Data();
    public Data data => _data;

    public Action<string> onIdChanged;
    public Action<int> onQuantityChanged;


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

    public void ResetPlant()
    {
        if (data.id == "")
            return;

        data.id = "";

        if (onIdChanged != null)
            onIdChanged.Invoke(id);

        ResetQuantity();
    }
    public bool SetPlant(string id)
    {
        if (data.id == id)
            return false;

        data.id = id;

        if (onIdChanged != null)
            onIdChanged.Invoke(id);

        return true;
    }

    public void ResetQuantity() => SetQuantity(0);
    public void SetQuantity(int quantity)
    {
        if (data.quantity == quantity)
            return;

        data.quantity = quantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);
    }
    public int AddPlant(int quantity)
    {
        if (quantity == 0)
            return 0;

        quantity = Mathf.Clamp(quantity, 0, maxQuantity);

        int previousQuantity = this.quantity;
        int futureQuantity = Mathf.Clamp(previousQuantity + quantity, 0, maxQuantity);

        data.quantity = futureQuantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);

        return futureQuantity - previousQuantity;

    }
    public int TakePlants(int quantity)
    {
        UI.Inventory.Instance.Select(id);

        if (quantity == 0)
        {
            return 0;
        }

        int previousQuantity = data.quantity;

        int futureQuantity = Mathf.Clamp(previousQuantity - quantity, 0, maxQuantity);

        data.quantity = futureQuantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);

        return previousQuantity - futureQuantity;
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

        if (onIdChanged != null)
            onIdChanged.Invoke(data.id);

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(data.quantity);
    }


    [Serializable]
    public class Data
    {
        public int maxQuantity = 99;
        public string id = null;
        public int quantity = 0;
    }
}