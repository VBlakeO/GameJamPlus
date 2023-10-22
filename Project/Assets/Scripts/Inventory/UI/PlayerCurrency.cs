using UnityEngine;
using TMPro;

namespace UI
{
    public class PlayerCurrency : Singleton<PlayerCurrency>
    {
        [SerializeField] TextMeshProUGUI _currencyText;
        public TextMeshProUGUI currencyText => _currencyText;


        void Start()
        {
            currencyText.text = "$ " + global::PlayerCurrency.Instance.amount.ToString();
            global::PlayerCurrency.Instance.onAmountChanged += (quantity) => currencyText.text = "$ " + quantity.ToString();
        }
    }
}