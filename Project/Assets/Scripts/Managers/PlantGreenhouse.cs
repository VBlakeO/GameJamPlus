using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class PlantGreenhouse : MonoBehaviour
{
    public List<PlantingSoil> plantingSoils = null;
    public string lastPlantationId = "";
    private int _seedsPlanted = 0;
    public Action OnFertilize = null;
    public Action OnHarvest = null;
  
    private void Awake() {

        foreach (var item in GetComponentsInChildren<PlantingSoil>())
            plantingSoils.Add(item);
    }

    public void FertilizePlantingSoils()
    {
        int _seedsAvailable = Inventory.Instance.TakePlant(UI.Inventory.Instance.selected, plantingSoils.Count);
        lastPlantationId = UI.Inventory.Instance.selected;

        if (_seedsAvailable != 0)
        {
            for (int i = 0; i < plantingSoils.Count; i++)
            {
                if(i <= _seedsAvailable)
                {
                    plantingSoils[i].Interact();
                    _seedsPlanted++;
                }
            }

            OnFertilize?.Invoke();
        }
    }

    public void HarvestPlantingSoils()
    {
        foreach (var item in plantingSoils)
        {
            if (item.fertilizedLand)
            {
                if (item.plant.PlantState == PlantState.ROOT)
                    SilosManager.Instance.AddPlant(lastPlantationId, _seedsPlanted);

                item.Interact();
            }
        }
    	
        OnHarvest?.Invoke();
        _seedsPlanted = 0;
    }

    public bool PlantGreenhouseFertilized()
    {
        bool check = false;
        for (int i = 0; i < plantingSoils.Count; i++)
        {
            if(plantingSoils[i].fertilizedLand)
                check = true;
        }

        return check;
    }

    public bool HasHealthyPlants()
    {
        bool healthyPlants = true;

        for (int i = 0; i < _seedsPlanted; i++)
        {
            if (plantingSoils[i].plant.PlantState == PlantState.LOST)
                healthyPlants = false;
        }

       return PlantGreenhouseFertilized() && healthyPlants;
    }

    public void DryPlants()
    {
        foreach (var item in plantingSoils)
            item.ActiveLostState();
    }
}
