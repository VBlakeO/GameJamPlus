using System;
using UnityEngine;

public class Silo : MonoBehaviour
{
    public int maxQuantity => data.maxQuantity;
    public string id => data.id;
    public int quantity => data.quantity;
    public int emptyQuantity => maxQuantity - quantity;

    [SerializeField] Data _data = new Data();
    public Data data => _data;

    [SerializeField] Transform siloContentTransform;

    public Action<string> onIdChanged;
    public Action<int> onQuantityChanged;


    void Start()
    {
        SilosManager.Instance.Add(this);

        siloContentTransform.localScale = new Vector3(1, quantity / maxQuantity, 1);
        onQuantityChanged += (quantity) => siloContentTransform.localScale = new Vector3(1, quantity / maxQuantity, 1);
    }
    void OnDestroy()
    {
        SilosManager.Instance.Remove(this);
    }

    public void ResetPlant()
    {
        if (data.id == "")
            return;

        data.id = "";

        if (onIdChanged != null)
            onIdChanged.Invoke(id);

        ResetQuantity();
    }
    public bool SetPlant(string id)
    {
        if (data.id == id)
            return false;

        data.id = id;

        if (onIdChanged != null)
            onIdChanged.Invoke(id);

        return true;
    }

    public void ResetQuantity() => SetQuantity(0);
    public void SetQuantity(int quantity)
    {
        if (data.quantity == quantity)
            return;

        data.quantity = quantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);
    }
    public int AddPlant(int quantity)
    {
        if (quantity == 0)
            return 0;

        quantity = Mathf.Clamp(quantity, 0, maxQuantity);

        int previousQuantity = this.quantity;
        int futureQuantity = Mathf.Clamp(previousQuantity + quantity, 0, maxQuantity);

        data.quantity = futureQuantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);

        return futureQuantity - previousQuantity;

    }
    public int TakePlants(int quantity)
    {
        UI.Inventory.Instance.Select(id);

        if (quantity == 0)
        {
            return 0;
        }

        int previousQuantity = data.quantity;

        int futureQuantity = Mathf.Clamp(previousQuantity - quantity, 0, maxQuantity);

        data.quantity = futureQuantity;

        if (onQuantityChanged != null)
            onQuantityChanged.Invoke(quantity);

        return previousQuantity - futureQuantity;
    }

    [Serializable]
    public class Data
    {
        public int maxQuantity = 99;
        public string id = null;
        public int quantity = 0;
    }
}