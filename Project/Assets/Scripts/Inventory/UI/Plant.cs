using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class Plant : MonoBehaviour
    {
        [SerializeField] string _id;
        public string id => _id;

        [SerializeField] Image _iconImage;
        public Image iconImage => _iconImage;

        [SerializeField] TextMeshProUGUI _quantityText;
        public TextMeshProUGUI quantityText => _quantityText;

        [SerializeField] GameObject selected;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => Inventory.Instance.Select(id));
        }

        public void Set(string id, int quantity)
        {
            this._id = id;
            iconImage.sprite = PlantStaticsHolder.Instance.plantStatics[id].icon;
            SetQuantity(quantity);
        }

        public void SetQuantity(int quantity)
        {
            quantityText.text = quantity.ToString();
            quantityText.gameObject.SetActive(quantity > 0);
        }

        public void OnSelectedChanged(bool b)
        {
            if (b)
                OnSelected();
            else
                OnDeselected();
        }

        public void OnSelected()
        {
            selected.SetActive(true);
        }

        public void OnDeselected()
        {
            selected.SetActive(false);
        }
    }
}