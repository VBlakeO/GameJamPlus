using System;
using UnityEngine;

public enum PlantState {NONE = 0, BUD = 1, FOLIAGE = 2, ROOT = 3}
public class PlantingSoil : MonoBehaviour, IInteract
{   
    public Plant plant = null;
    public GameObject currentPlantObj = null;
    private bool fertilizedLand => plant != null;
    
    public void Interact()
    {
        if (!fertilizedLand)
            Fertilize("samplePlant");
        else
            Harvest();
    }

    private PlantState CheckPlantState()
    {
        if (fertilizedLand)
            return plant.PlantState;
        else
            return PlantState.NONE;
    }

    public void Fertilize(string id)
    {
        plant = new Plant(id);
        plant.PlantState = PlantState.BUD;

        currentPlantObj = PoolingManager.Instance.plants[id][0].Pool.Get();
        currentPlantObj.transform.position = transform.position;
    }

    public void Harvest()
    {   
        if(!currentPlantObj)
        return;

        currentPlantObj.SetActive(false);
        currentPlantObj = null;
        plant = null;
    } 

    // folhagem seca e raiz apodrece
}
