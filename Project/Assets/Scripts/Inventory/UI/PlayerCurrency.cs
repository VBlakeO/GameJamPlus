using UnityEngine;
using TMPro;

namespace UI
{
    public class PlayerCurrency : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI currencyText;


        void Start()
        {
            global::PlayerCurrency.Instance.onAmountChanged += (quantity) => currencyText.text = "$ " + quantity.ToString();
        }
    }
}