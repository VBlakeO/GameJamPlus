using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    public GameObject poolingSystemPrefab = null;
    public Dictionary<string, List<PoolingSystem>> plants = new Dictionary<string, List<PoolingSystem>>();
    
    void Start()
    {
        foreach (var item in PlantStaticsHolder.Instance.plantStatics)
        {
            plants.Add(item.Value.id, new List<PoolingSystem>());

            foreach (var plant in item.Value.plantStateObjects)
            {
                var poolingSystem = Instantiate(poolingSystemPrefab, transform).GetComponent<PoolingSystem>();
                poolingSystem.id = item.Value.id;
                poolingSystem.plant = plant;
                plants[item.Value.id].Add(poolingSystem);
            }
        }
    }

}
