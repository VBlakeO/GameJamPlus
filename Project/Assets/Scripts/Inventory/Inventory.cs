using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] int maxQuantity = 99;
    [SerializeField] Dictionary<string, int> plants; //<id, quantity>


    public bool AddPlant(string id)
    {
        if (plants.ContainsKey(id))
        {
            if (plants[id] < plants[id])
                plants[id]++;
            else //reached max qnt
                return false;
        }
        else
            plants.Add(id, 1);

        return true;
    }

    public bool RemovePlant(string id)
    {
        if (plants.ContainsKey(id))
        {
            if (plants[id] > 0)
            {
                plants[id]--;
                return true;
            }
        }

        return false;
    }
}