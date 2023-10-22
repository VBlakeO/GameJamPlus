using UnityEngine;

public class PlantingSoilGrup : MonoBehaviour, IInteract
{
    [SerializeField] private PlantGreenhouse plantGreenhouse = null;

    public void Interact()
    {
        if (!plantGreenhouse.PlantGreenhouseFertilized())
        {
            plantGreenhouse.FertilizePlantingSoils();
        }
        else
        {
            plantGreenhouse.HarvestPlantingSoils();
        }
    }
}