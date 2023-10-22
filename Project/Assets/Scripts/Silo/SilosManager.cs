using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SilosManager : Singleton<SilosManager>, IPersistent
{
    string _persistentPath = "/Silos";

    [SerializeField] Data _data = new Data();
    public Data data => _data;

    public List<Silo> silos => _data.silos;

    public Action<List<Silo>> onSilosChanged;
    public Action<Silo> onSiloAdded;
    public Action<Silo> onSiloRemoved;

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

    public void Add(Silo newSilo)
    {
        silos.Add(newSilo);

        if (onSiloAdded != null)
            onSiloAdded.Invoke(newSilo);
    }
    public void Remove(Silo oldSilo)
    {
        silos.Remove(oldSilo);

        if (onSiloRemoved != null)
            onSiloRemoved.Invoke(oldSilo);
    }

    public int AddPlant(string id, int quantity)
    {
        if (quantity == 0)
            return 0;

        int remaining = quantity;

        List<Silo> silosWithThisPlant = new List<Silo>();
        List<Silo> silosEmpty = new List<Silo>();

        foreach (var silo in silos)
        {
            if (silo.id == id && silo.emptyQuantity > 0)
            {
                silosWithThisPlant.Add(silo);
            }
            else
            {
                if (!String.IsNullOrEmpty(silo.id))
                    continue;

                silosEmpty.Add(silo);
            }
        }

        foreach (var silo in silosWithThisPlant)
        {
            remaining -= silo.AddPlant(remaining);

            if (remaining <= 0)
                return quantity;
        }

        foreach (var silo in silosEmpty)
        {
            silo.SetPlant(id);
            remaining -= silo.AddPlant(quantity);

            if (remaining <= 0)
                return quantity;
        }


        return quantity - remaining;
    }
    public bool AddPlant(string id)
    {
        return AddPlant(id, 1) >= 1;
    }

    public int TakePlant(string id, int quantity)
    {
        if (quantity == 0)
            return 0;

        int remaining = quantity;

        foreach (var silo in silos)
        {
            if (silo.id == id && silo.emptyQuantity > 0)
            {
                remaining -= silo.TakePlants(remaining);

                if (remaining <= 0)
                    return quantity;
            }
        }

        return quantity - remaining;
    }
    public bool TakePlant(string id)
    {
        return TakePlant(id, 1) >= 1;
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

        if (onSilosChanged != null)
            onSilosChanged.Invoke(_data.silos);
    }


    [Serializable]
    public class Data
    {
        public List<Silo> silos;
    }
}