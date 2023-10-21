using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant")]
public class PlantStatic : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite icon;
}