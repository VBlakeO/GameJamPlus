using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant")]
public class PlantStatic : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite icon;
    public int buyPrice = 1;
    public int sellPrice = 3;
    public string description = "plant description";

    [Header("StatesDuration")]
    public float budStateDuration = 1f;
    public float foliageStateDuration = 1f;
    public float rootStateDuration = 1f;
    [Space]

    [Header("ConsumptionSpeed")]
    public float consumptionSpeed = 1f;

    public GameObject[] plantStateObjects; 
}