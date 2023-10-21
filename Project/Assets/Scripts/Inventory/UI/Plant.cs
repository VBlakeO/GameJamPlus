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

        [SerializeField] Image _icon;
        public Image icon => _icon;

        [SerializeField] TextMeshProUGUI _quantity;
        public TextMeshProUGUI quantity => _quantity;

        [SerializeField] GameObject selected;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => Inventory.Instance.Select(id));
        }

        public void Set(string id)
        {
            this._id = id;
            icon.sprite = PlantStaticsHolder.Instance.plantStatics[id].icon;
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