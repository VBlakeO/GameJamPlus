using DG.Tweening;
using UI;
using UnityEngine;

public class SeedShop : Singleton<SeedShop>
{
    [SerializeField] SeedPanel[] panels;

    protected override void Awake()
    {
        base.Awake();

        if (panels.Length != PlantStaticsHolder.Instance.plantStatics.Count)
        {
            Debug.LogError("The amount of plants and the amount of seed panels are different");
        }
    }

    void Start()
    {
        InitPanels();
    }

    void InitPanels()
    {
        int i = 0;

        foreach (PlantStatic plantStatic in PlantStaticsHolder.Instance.plantStatics.Values)
        {
            panels[i].Set(plantStatic.id);
            i++;
        }
    }

    public void Buy(string id, int quantity)
    {
        Debug.Log("Trying to buy " + id + " " + quantity + ". Lets see...");

        if (!PlantStaticsHolder.Instance.plantStatics.ContainsKey(id))
        {
            Debug.Log("Trying to buy an unknown plant (" + id  + "). Aborting action...");
            return;
        }

        if (quantity <= 0)
        {
            Debug.Log("Trying to buy 0 seeds. Aborting action...");
            return;
        }

        int buyPrice = quantity * PlantStaticsHolder.Instance.plantStatics[id].buyPrice;

        PlayerCurrency playerCurrency = PlayerCurrency.Instance;

        if (playerCurrency.amount < buyPrice)
        {
            Debug.Log("Player doesnt have enough currency ($) to buy " +  quantity + " " + id + ". Aborting action...");
           UI.PlayerCurrency.Instance.currencyText.rectTransform.DOShakePosition(.5f, 8);
            return;
        }

        Inventory.Instance.AddPlant(id, quantity);
        playerCurrency.Remove(buyPrice);
    }
}