using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class Plant : MonoBehaviour
    {
        [SerializeField] string id;
        [SerializeField] Image image;

        [SerializeField] GameObject selected;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => Inventory.Instance.Select(id));
        }

        public void Set(string id)
        {
            this.id = id;
            image.sprite = PlantStaticsHolder.Instance.plantStatics[id].icon;
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