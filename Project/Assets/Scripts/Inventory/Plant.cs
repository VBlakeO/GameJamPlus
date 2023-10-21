using UnityEngine;

[SerializeField]
public class Plant
{
    public string id => _static.id;
    public string name => _static.name;
    public Sprite icon => _static.icon;

    PlantStatic _static;


    public Plant(string id)
    {
        _static = PlantStaticsHolder.Instance.Get(id);

        //...
    }
}