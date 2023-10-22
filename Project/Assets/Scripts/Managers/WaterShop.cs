using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaterShop : Singleton<WaterShop>
{
    public int price = 1;

    int _quantity = 1;
    public int quantity
    {
        get => _quantity;
        private set
        {
            _quantity = value;
            quantityText.text = quantity.ToString();
            totalPriceText.text = "$ " + (quantity * price).ToString();
        }
    }

    [SerializeField] TextMeshProUGUI _quantityText;
    public TextMeshProUGUI quantityText => _quantityText;

    [SerializeField] TextMeshProUGUI _unitPriceText;
    public TextMeshProUGUI unitPriceText => _unitPriceText;

    [SerializeField] TextMeshProUGUI _totalPriceText;
    public TextMeshProUGUI totalPriceText => _totalPriceText;

    protected override void Awake()
    {
        base.Awake();

        unitPriceText.text = "$ " + price;
        quantity = 1;
    }

    public void AddQuantityOne() => AddQuantity(1);
    public void AddQuantity(int quantity)
    {
        this.quantity += quantity;
    }

    public void RemoveQuantityOne() => RemoveQuantity(1);
    public void RemoveQuantity(int quantity)
    {
        if (this.quantity - quantity <= 0)
            return;

        this.quantity -= quantity;
    }

    public void Buy()
    {
        int buyPrice = quantity * price;

        PlayerCurrency playerCurrency = PlayerCurrency.Instance;

        if (playerCurrency.amount < buyPrice)
        {
            Debug.Log("Player doesnt have enough currency ($) to buy " + quantity + " water. Aborting action...");
            UI.PlayerCurrency.Instance.currencyText.rectTransform.DOShakePosition(.5f, 8);
            return;
        }

        WaterReservatory.Instance.AddWater(quantity);
        playerCurrency.Remove(buyPrice);
    }
}