using Unity.Mathematics;
using UnityEngine;

public enum PlantState {BUD, FOLIAGE, ROOT, NONE}
public class PlantingSoil : MonoBehaviour, IInteract
{   
    public Plant plant = null;
    private bool fertilizedLand => plant != null;

    public void Interact()
    {
        print("Plantar");
        Fertilize("samplePlant");
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

        GameObject plantObj = PoolingManager.Instance.plants[id][0].Pool.Get();
        plantObj.transform.position = transform.position;
    }

    public void Harvest()
    {
        
        
       
    } 

    // folhagem seca e raiz apodrece
}
