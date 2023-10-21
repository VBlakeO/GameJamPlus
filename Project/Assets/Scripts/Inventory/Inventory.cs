using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Inventory : Singleton<Inventory>, IPersistent
{
    string _persistentPath = "/Inventory";

    [SerializeField] int maxQuantity = 99;
     Dictionary<string, int> plants => data.plants; //<id, quantity>

    [SerializeField] Data _data = new Data();
    public Data data => _data;

    public Action<string, int> onPlantQuantityChanged;
    public Action<string> onPlantTakeFailed;

    public Action<Data> onInventoryChanged;


    protected override void Awake()
    {
        base.Awake();

        AddPlant("samplePlant", 8);
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

    public int AddPlant(string id, int quantity)
    {
        if (quantity == 0)
            return 0;

        quantity = Mathf.Clamp(quantity, 0, maxQuantity);

        if (plants.ContainsKey(id))
        {
            int previousQuantity = plants[id];

            int futureQuantity = Mathf.Clamp(previousQuantity + quantity, 0, maxQuantity);

            plants[id] = futureQuantity;

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return futureQuantity - previousQuantity;
        }
        else
        {
            plants.Add(id, quantity);

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return quantity;
        }
    }
    public bool AddPlant(string id)
    {
        if (plants.ContainsKey(id))
        {
            if (plants[id] < plants[id])
            {
                plants[id]++;

                if (onPlantQuantityChanged != null)
                    onPlantQuantityChanged.Invoke(id, plants[id]);
            }
            else //reached max qnt
                return false;
        }
        else
        {
            plants.Add(id, 1);

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);
        }

        return true;
    }

    public int TakePlants(string id, int quantity)
    {
        UI.Inventory.Instance.Select(id);

        if (quantity == 0)
        {
            if (onPlantTakeFailed != null)
                onPlantTakeFailed.Invoke(id);

            return 0;
        }

        if (plants.ContainsKey(id))
        {
            int previousQuantity = plants[id];

            int futureQuantity = Mathf.Clamp(previousQuantity - quantity, 0, maxQuantity);

            plants[id] = futureQuantity;

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return previousQuantity - futureQuantity;
        }

        if (onPlantTakeFailed != null)
            onPlantTakeFailed.Invoke(id);

        return 0;
    }
    public bool TakePlant(string id)
    {
        UI.Inventory.Instance.Select(id);

        if (plants.ContainsKey(id))
        {
            if (plants[id] > 0)
            {
                plants[id]--;


                if (onPlantQuantityChanged != null)
                    onPlantQuantityChanged.Invoke(id, plants[id]);

                return true;
            }
        }

        if (onPlantTakeFailed != null)
            onPlantTakeFailed.Invoke(id);

        return false;
    }

    bool IPersistent.HasJsonSave(string persistentDataPath)
    {
        return File.Exists(persistentDataPath + _persistentPath);
    }
    void IPersistent.SaveAsJson(string persistentDataPath)
    {
        File.WriteAllText(persistentDataPath + _persistentPath, JsonConvert.SerializeObject(_data));
    }
    void IPersistent.LoadFromJson(string persistentDataPath)
    {
        if (!((IPersistent)this).HasJsonSave(persistentDataPath))
        {
            ((IPersistent)this).SaveAsJson(persistentDataPath);
            return;
        }

        _data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(persistentDataPath + _persistentPath));

        if (onInventoryChanged != null)
            onInventoryChanged.Invoke(_data);
    }


    [Serializable]
    public class Data
    {
        public Dictionary<string, int> plants = new Dictionary<string, int>(); //<id, quantity>

    }
}