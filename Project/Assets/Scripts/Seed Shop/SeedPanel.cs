using UnityEngine;
using UnityEngine.UI;           
using TMPro;

public class SeedPanel : MonoBehaviour
{
    public string id;
    public int quantity = 1;

    [SerializeField] TextMeshProUGUI _quantityText;
    public TextMeshProUGUI quantityText => _quantityText;

    [SerializeField] Image _iconImage;
    public Image icon => _iconImage;


    public void AddQuantityOne() => AddQuantity(1);
    public void AddQuantity(int quantity)
    {
        this.quantity += quantity;
        quantityText.text = this.quantity.ToString();
    }

    public void RemoveQuantityOne() => RemoveQuantity(1);
    public void RemoveQuantity(int quantity)
    {
        if (this.quantity - quantity <= 0)
            return;

        this.quantity -= quantity;
        quantityText.text = this.quantity.ToString();
    }

    public void Buy()
    {
        SeedShop.Instance.Buy(id, quantity);
    }
}