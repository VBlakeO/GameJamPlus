using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] bool _isOpen = false;
        [Space]
        [SerializeField] RectTransform _tweenTransform;
        float _tweenStartPositionY;
        [SerializeField] float _tweenTargetPositionY;
        [SerializeField] float _tweenDuration = .5f;
        [Space]
        [SerializeField] Transform _itemsContent;
        [SerializeField] GameObject _itemPrefab;
        [Space]
        Dictionary<string, Plant> _plantsUI;
        [SerializeField] List<(string, int)> startPlants;
        [Space]
        [SerializeField] string _selected;
        public string selected => _selected;

        [SerializeField] GameObject selectedDisplay;
        [SerializeField] Image selectedIcon;

        public Action<string> onSelectedChanged;


        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            _tweenStartPositionY = transform.position.y;


        }

        void Start()
        {
            InstantiatePlantsUI();

            UpdatePlantsQuantity(global::Inventory.Instance.plants);

            global::Inventory.Instance.onInventoryChanged += (data) => UpdatePlantsQuantity(data.plants);
            global::Inventory.Instance.onPlantQuantityChanged += (id, value) => UpdatePlantQuantity(id, value);
            global::Inventory.Instance.onPlantTakeFailed += (id) => selectedDisplay.GetComponent<RectTransform>().DOShakePosition(.5f, 5);

            SelectFirstAvailable();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                SwitchState();
        }
        #endregion Unity Callbacks


        void InstantiatePlantsUI()
        {
            _plantsUI = new Dictionary<string, Plant>();

            GameObject instance = null;

            foreach (KeyValuePair<string, PlantStatic> item in PlantStaticsHolder.Instance.plantStatics)
            {
                instance = MonoBehaviour.Instantiate(_itemPrefab, _itemsContent);

                Plant plant = instance.GetComponent<Plant>();
                plant.Set(item.Key);
                _plantsUI.Add(item.Key, plant);
                instance.SetActive(false);
            }
        }

        void UpdatePlantsQuantity() => UpdatePlantsQuantity(global::Inventory.Instance.plants);
        void UpdatePlantsQuantity(Dictionary<string, int> data)
        {
            foreach (var plant in data)
            {
                UpdatePlantQuantity(plant.Key, plant.Value);
            }
        }

        void UpdatePlantQuantity(string id)
        {
            int quantity = global::Inventory.Instance.plants[id];
            UpdatePlantQuantity(id, quantity);
        }
        void UpdatePlantQuantity(string id, int quantity)
        {
            _plantsUI[id].quantity.text = quantity.ToString();
            _plantsUI[id].gameObject.SetActive(quantity > 0);

            if (String.IsNullOrEmpty(selected) || (selected == id && quantity == 0))
                SelectFirstAvailable();
        }

        void SelectFirstAvailable()
        {
            foreach (var plant in global::Inventory.Instance.plants)
            {
                if (plant.Value > 0)
                {
                    Select(plant.Key);
                    return;
                }
            }

            Select("");
        }

        public void SetState(bool b)
        {
            if (_isOpen == b)
                return;

            SwitchState();
        }
        public void SwitchState()
        {
            _isOpen = !_isOpen;

            if (_isOpen)
                Show();
            else
                Hide();
        }
        public void Show()
        {
            _tweenTransform.DOAnchorPos3DY(_tweenTargetPositionY, _tweenDuration);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        public void Hide()
        {
            _tweenTransform.DOAnchorPos3DY(_tweenStartPositionY, _tweenDuration);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Select(string id)
        {
            if (id == _selected)
                return;

            if (!global::Inventory.Instance.plants.TryGetValue(id, out _))
            {
                Debug.Log("Trying to select '" + id + "' failed because it doesnt exist");
                return;
            }

            if (global::Inventory.Instance.plants[id] <= 0) //quantity 0
            {
                Debug.Log("Trying to select '" + id + "' failed because quantity is 0");
                return;
            }

            if (!String.IsNullOrEmpty(_selected))
                _plantsUI[_selected].OnDeselected();

            _selected = id;

            _plantsUI[_selected].OnSelected();

            if (!String.IsNullOrEmpty(_selected))
            {
                selectedIcon.sprite = PlantStaticsHolder.Instance.plantStatics[id].icon;
                selectedIcon.enabled = true;
            }
            else
            {
                selectedIcon.sprite = null;
                selectedIcon.enabled = false;
            }


            if (onSelectedChanged != null)
                onSelectedChanged.Invoke(id);
        }
    }
}