using System.Runtime.InteropServices;
using UnityEngine;

[SerializeField]
public class Plant
{
    public string id => _static.id;
    public string name => _static.name;
    public Sprite icon => _static.icon;

    public float budStateDuration => _static.budStateDuration;
    public float foliageStateDuration => _static.foliageStateDuration;
    public float rootStateDuration => _static.rootStateDuration;

    public float consumptionSpeed => _static.consumptionSpeed;

    public GameObject[] plantStateObjects => _static.plantStateObjects;
    private PlantStatic _static;

    public float currentTime;
    public PlantState PlantState = PlantState.BUD;

    public Plant(string id)
    {
        _static = PlantStaticsHolder.Instance.Get(id);
        //...
    }
}