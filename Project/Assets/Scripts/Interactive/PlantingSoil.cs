using System;
using UnityEngine;

public enum PlantState {NONE = 0, BUD = 1, FOLIAGE = 2, ROOT = 3}
public class PlantingSoil : MonoBehaviour, IInteract
{   
    public Plant plant = null;
    public float t_currentTime;
    public PlantState t_currentState;
    public GameObject currentPlantObj = null;

    private string currentId = null;
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

    void FixedUpdate()
    {
        if (!fertilizedLand)
            return;

        if ((int)plant.PlantState < Enum.GetNames(typeof(PlantState)).Length)
        {
            float currentStateDuration = plant.PlantState switch
            {
                PlantState.BUD => plant.budStateDuration,
                PlantState.FOLIAGE => plant.foliageStateDuration,
                PlantState.ROOT => plant.rootStateDuration,
                PlantState.NONE => 0f,
                _ => 0f,
            };

            float multiplicador = 1f / currentStateDuration;

            if (plant.currentTime < 1f)
            {
                plant.currentTime += Time.deltaTime * multiplicador;
            }
            else
            {
                if (plant.PlantState != PlantState.ROOT)
                {
                    plant.currentTime = 0f;
                    plant.PlantState++;
                    ChangePlantObject();
                }
            }
        }

        t_currentTime = plant.currentTime;
        t_currentState = plant.PlantState;
    }

    private void ChangePlantObject()
    {
        currentPlantObj.SetActive(false);
        currentPlantObj = PoolingManager.Instance.plants[currentId][(int)plant.PlantState-1].Pool.Get();
        currentPlantObj.transform.position = transform.position;
    }

    public void Fertilize(string id)
    {
        plant = new Plant(id);
        currentId = id;
        plant.PlantState = PlantState.BUD;

        currentPlantObj = PoolingManager.Instance.plants[id][0].Pool.Get();
        currentPlantObj.transform.position = transform.position;

        print("Fertilize");
    }

    public void Harvest()
    {   
        if(!currentPlantObj || plant.PlantState != PlantState.ROOT)
        return;

        currentPlantObj.SetActive(false);
        currentPlantObj = null;
        currentId = "";
        plant = null;

        print("Harvest");
    }

    // folhagem seca e raiz apodrece
}
