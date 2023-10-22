using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class PlantGreenhouse : MonoBehaviour
{
    public List<PlantingSoil> plantingSoils = null;
    public string lastPlantationId = "";
    private int _seedsPlanted = 0;
    private int _currentButton = 0;
    [SerializeField] private GameObject[] buttonsObj;
    [SerializeField] private BoxCollider[] buttonsCollider;

    public Action OnFertilize = null;
    public Action OnHarvest = null;
  
    private void Awake() {

        foreach (var item in GetComponentsInChildren<PlantingSoil>())
            plantingSoils.Add(item);
    }

    private void Start() 
    {
        plantingSoils[0].OnReadyToHarvest += ReadyToHarvest;
        plantingSoils[0].OnLostHarvest += LostHarvest;
        ChangeButton(0);
    }

    public void FertilizePlantingSoils()
    {
        int _seedsAvailable = Inventory.Instance.TakePlant(UI.Inventory.Instance.selected, plantingSoils.Count);
        lastPlantationId = UI.Inventory.Instance.selected;

        if (_seedsAvailable == 0)
            return;

        for (int i = 0; i < plantingSoils.Count; i++)
        {
            if (i < _seedsAvailable)
            {
                plantingSoils[i].Interact();
                _seedsPlanted++;
            }
        }

        OnFertilize?.Invoke();
        ChangeButton(1);
    }

    public void HarvestPlantingSoils()
    {
        if (plantingSoils[0].plant.PlantState == PlantState.ROOT)
            SilosManager.Instance.AddPlant(lastPlantationId, _seedsPlanted);

        foreach (var item in plantingSoils)
        {
            if (item.fertilizedLand)
                item.Interact();
        }

        OnHarvest?.Invoke();
        _seedsPlanted = 0;
        ChangeButton(0);
    }


    private void ChangeButton(int button)
    {
        DisableAllButton();
        _currentButton = button;

        buttonsObj[button].SetActive(true);

        if (button == 1)
            buttonsCollider[1].enabled = true;
        else
            buttonsCollider[0].enabled = true;
    }

    private void DisableAllButton()
    {
        for (int i = 0; i < buttonsObj.Length; i++)
        {
            buttonsObj[i].SetActive(false);
        }

        buttonsCollider[0].enabled = false;
        buttonsCollider[1].enabled = false;
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
        
        ChangeButton(3);
    }

    private void ReadyToHarvest()
    {
        ChangeButton(2);
    }

    private void LostHarvest()
    {
        ChangeButton(3);
    }
}
