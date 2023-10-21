using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant")]
public class PlantStatic : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite icon;

    [Header("StatesDuration")]
    public float budStateDuration = 1f;
    public float foliageStateDuration = 1f;
    public float rootStateDuration = 1f;
    [Space]

    public GameObject[] plantStateObjects; 
}