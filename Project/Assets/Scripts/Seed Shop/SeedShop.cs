using DG.Tweening;
using UnityEngine;

public class SeedShop : Singleton<SeedShop>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void Buy(string id, int quantity)
    {
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
           UI.PlayerCurrency.Instance.currencyText.rectTransform.DOShakePosition(.5f, 5);
            return;
        }

        Inventory.Instance.AddPlant(id, quantity);
        playerCurrency.Remove(buyPrice);
    }
}