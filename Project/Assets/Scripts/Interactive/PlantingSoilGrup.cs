using System.Collections.Generic;
using UnityEngine;

public class PlantingSoilGrup : MonoBehaviour, IInteract
{
    public List<PlantingSoil> plantingSoils = null;


    private void Awake() {

        foreach (var item in GetComponentsInChildren<PlantingSoil>())
            plantingSoils.Add(item);
    }

    public void Interact()
    {
        int _seedsAvailable = Inventory.Instance.TakePlants(UI.Inventory.Instance.selected, plantingSoils.Count);
        
        if (_seedsAvailable == 0)
            return;

        if (_seedsAvailable < plantingSoils.Count)
        {
            for (int i = 0; i < _seedsAvailable; i++)
                plantingSoils[i].Interact();
        }
        else
        {
            foreach (var item in plantingSoils)
                item.Interact();
        }
    }
}
