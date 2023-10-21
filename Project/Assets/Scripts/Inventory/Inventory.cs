using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using Newtonsoft.Json;


public class Inventory : Singleton<Inventory>
{
    [SerializeField] string _persistentPath = "/Inventory";

    [SerializeField] int maxQuantity = 99;
     Dictionary<string, int> plants => data.plants; //<id, quantity>

    [SerializeField] Data _data = new Data();
    public Data data => _data;

    public Action<string, int> onPlantQuantityChanged;
    public Action<string> onPlantTakeFailed;

    protected override void Awake()
    {
        base.Awake();

        Persistent.Instance.onSave += SaveAsJson;
        Persistent.Instance.onLoad += LoadFromJson;
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


    public void LoadFromJson(string persistentDataPath)
    {
        _data = JsonConvert.DeserializeObject<Data>(System.IO.File.ReadAllText(persistentDataPath + _persistentPath + ".json"));
    }

    public void SaveAsJson(string persistentDataPath)
    {
        System.IO.File.WriteAllText(persistentDataPath + _persistentPath + ".json", JsonConvert.SerializeObject(_data));
    }

    [Serializable]
    public class Data
    {
        public Dictionary<string, int> plants = new Dictionary<string, int>(); //<id, quantity>

    }
}