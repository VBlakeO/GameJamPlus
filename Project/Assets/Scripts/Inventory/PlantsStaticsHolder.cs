using System.Collections.Generic;
using UnityEngine;

public class PlantStaticsHolder : Singleton<PlantStaticsHolder>
{
    [SerializeField] List<PlantStatic> _plantStatics;
    public Dictionary<string, PlantStatic> plantStatics { get; private set; }

    public PlantStatic Get(string id) => plantStatics[id];

    protected override void Awake()
    {
        base.Awake();
        InitializeDictionary();
    }

    void InitializeDictionary()
    {
        plantStatics = new Dictionary<string, PlantStatic>();
        foreach (var plant in _plantStatics)
        {
            plantStatics[plant.id] = plant;
        }
    }
}