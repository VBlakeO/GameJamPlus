using UnityEngine;
using UnityEngine.UI;           
using TMPro;

public class SeedPanel : MonoBehaviour
{
    public PlantStatic plantStatic => PlantStaticsHolder.Instance.plantStatics[id];

    [SerializeField] string _id;
    public string id
    {
        get => _id;
        private set
        {
            _id = value;


            nameText.text = plantStatic.name;
            iconImage.sprite = plantStatic.icon;
            unitPriceText.text = "$ " + plantStatic.buyPrice.ToString();

            descriptionText.text = plantStatic.description;
            quantity = 1;
        }
    }
    public int priceUnit => plantStatic.buyPrice;

    int _quantity = 1;
    public int quantity
    {
        get => _quantity;
        private set
        {
            _quantity = value;
            quantityText.text = quantity.ToString();
            totalPriceText.text = "$ " + (quantity * plantStatic.buyPrice).ToString();
        }
    }

    [SerializeField] TextMeshProUGUI _quantityText;
    public TextMeshProUGUI quantityText => _quantityText;

    [SerializeField] TextMeshProUGUI _unitPriceText;
    public TextMeshProUGUI unitPriceText => _unitPriceText;

    [SerializeField] TextMeshProUGUI _totalPriceText;
    public TextMeshProUGUI totalPriceText => _totalPriceText;

    [SerializeField] Image _iconImage;
    public Image iconImage => _iconImage;

    [SerializeField] TextMeshProUGUI _nameText;
    public TextMeshProUGUI nameText => _nameText;

    [SerializeField] TextMeshProUGUI _descriptionText;
    public TextMeshProUGUI descriptionText => _descriptionText;


    public void Set(string plantStaticId)
    {
        this.id = plantStaticId;
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
        SeedShop.Instance.Buy(id, quantity);
    }
}