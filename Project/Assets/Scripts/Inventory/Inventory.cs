using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] int maxQuantity = 99;
    [SerializeField] Dictionary<string, int> plants; //<id, quantity>
    public Action<string, int> onPlantQuantityChanged;

    public int AddPlant(string id, int quantity)
    {
        if (quantity == 0)
            return 0;

        quantity = Mathf.Clamp(quantity, 0, maxQuantity);

        if (plants.ContainsKey(id))
        {
            int previousQuantity = plants[id];

            int futureQuantity = Mathf.Clamp(previousQuantity + quantity, 0, maxQuantity);

            plants[id] = futureQuantity;

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return futureQuantity - previousQuantity;
        }
        else
        {
            plants.Add(id, quantity);

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return quantity;
        }
    }


    public bool AddPlant(string id)
    {
        if (plants.ContainsKey(id))
        {
            if (plants[id] < plants[id])
            {
                plants[id]++;

                if (onPlantQuantityChanged != null)
                    onPlantQuantityChanged.Invoke(id, plants[id]);
            }
            else //reached max qnt
                return false;
        }
        else
        {
            plants.Add(id, 1);

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);
        }

        return true;
    }

    public int TakePlants(string id, int quantity)
    {
        if (quantity == 0)
            return 0;

        if (plants.ContainsKey(id))
        {
            int previousQuantity = plants[id];

            int futureQuantity = Mathf.Clamp(previousQuantity - quantity, 0, maxQuantity);

            plants[id] = futureQuantity;

            if (onPlantQuantityChanged != null)
                onPlantQuantityChanged.Invoke(id, plants[id]);

            return previousQuantity - futureQuantity;
        }

        return 0;
    }

    public bool TakePlant(string id)
    {
        if (plants.ContainsKey(id))
        {
            if (plants[id] > 0)
            {
                plants[id]--;


                if (onPlantQuantityChanged != null)
                    onPlantQuantityChanged.Invoke(id, plants[id]);

                return true;
            }
        }

        return false;
    }
}