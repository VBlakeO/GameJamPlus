using UnityEngine;

[SerializeField]
public class Plant
{
    public string id => _static.id;
    public string name => _static.name;
    public Sprite icon => _static.icon;
    public GameObject[] plantStateObjects => _static.plantStateObjects;

    PlantStatic _static;
    public PlantState PlantState {get; private set;}

    public Plant(string id)
    {
        _static = PlantStaticsHolder.Instance.Get(id);
        //...
    }
}