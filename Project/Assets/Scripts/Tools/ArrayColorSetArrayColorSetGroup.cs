using System.Collections.Generic;
using UnityEngine;

public class ArrayColorSetGroup : MonoBehaviour
{
    public List<RenderPlantColorControl> arrayColorSetList = new();
    private void Start() 
    {
        foreach (var item in GetComponentsInChildren<RenderPlantColorControl>())
            arrayColorSetList.Add(item);
    }

    public void SetNewColor()
    {
        foreach (var item in arrayColorSetList)
            item.ActivateDryColor();
    }

}