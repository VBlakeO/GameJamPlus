using System;
using UnityEngine;
using UnityEngine.Events;

public enum PlantState { NONE = 0, BUD = 1, FOLIAGE = 2, ROOT = 3, LOST = 4}
public class PlantingSoil : MonoBehaviour, IInteract
{
    public Plant plant = null;
    public GameObject currentPlantObj = null;
    public bool fertilizedLand => plant != null;
    private string currentId = null;

    public Action OnReadyToHarvest = null;
    public Action OnLostHarvest = null;

    [SerializeField] private MeshRenderer meshRenderer;


    public PlantState t_plantState = PlantState.NONE;

    private bool _readyToHarvest = false;


    public void Interact()
    {
        if (!fertilizedLand)
            Fertilize(UI.Inventory.Instance.selected);
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
        //meshRenderer.enabled = false;
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

        meshRenderer.enabled = true;
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
                if (plant.PlantState != PlantState.ROOT)
                {
                    if (_readyToHarvest)
                        _readyToHarvest = false;
                }

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

        if (plant.PlantState == PlantState.ROOT)
            OnReadyToHarvest?.Invoke();        
        
        if (plant.PlantState == PlantState.LOST)
            OnLostHarvest?.Invoke();
    }
  
    public void ActiveLostState()
    {
        if(plant != null)
            plant.PlantState = PlantState.LOST;
        SetDryColor();
    }

    private void SetDryColor()
    {
        if (currentPlantObj)
            currentPlantObj.GetComponent<ArrayColorSetGroup>().SetNewColor();
    }
}
