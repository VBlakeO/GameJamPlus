using System;
using UnityEngine;

public enum PlantState { NONE = 0, BUD = 1, FOLIAGE = 2, ROOT = 3, LOST = 4}
public class PlantingSoil : MonoBehaviour, IInteract
{
    public Plant plant = null;
    public GameObject currentPlantObj = null;
    public bool fertilizedLand => plant != null;
    public Color lostPlantColor = new();

    private Color[] previousColors = null;
    private string currentId = null;


public PlantState t_plantState = PlantState.NONE;

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
        currentId = id;
        plant.PlantState = PlantState.BUD;

        currentPlantObj = PoolingManager.Instance.plants[id][0].Pool.Get();
        currentPlantObj.transform.position = transform.position;
    }

    public void Harvest()
    {
        if (!currentPlantObj)
            return;

        if (plant.PlantState != PlantState.ROOT && plant.PlantState != PlantState.LOST)
            return;

        if (plant.PlantState == PlantState.ROOT)
        {
            HarvestEffect();
        }

        if (plant.PlantState == PlantState.LOST)
        {
            DestroyEffect();
        }

        currentPlantObj.SetActive(false);
        currentPlantObj = null;
        currentId = "";
        plant = null;
    }

    private void HarvestEffect()
    {

    }

    private void DestroyEffect()
    {

    }

    

    void FixedUpdate()
    {
        if (!fertilizedLand)
            return;

        UpdatePlanteState();
    }

    private void UpdatePlanteState()
    {
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
                if (plant.PlantState != PlantState.LOST)
                {
                    plant.currentTime = 0f;
                    plant.PlantState++;
                    ChangePlantObject();
                }
                else
                {
                    SetDryColor();
                }

                t_plantState = plant.PlantState;
            }
        }
    }

    private void ChangePlantObject()
    {
        currentPlantObj.SetActive(false);
        currentPlantObj = PoolingManager.Instance.plants[currentId][(int)plant.PlantState - 1].Pool.Get();
        currentPlantObj.transform.position = transform.position;
    }
  
    public void ActiveLostState()
    {
        plant.PlantState = PlantState.LOST;
        SetDryColor();
    }

    private void SetDryColor()
    {
        currentPlantObj.GetComponent<ArrayColorSetGroup>().SetNewColor();
    }
}
