using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        Dictionary<string, int> plants = new Dictionary<string, int>();
        [Space]
        [SerializeField] string _selected;
        public string selected => _selected;

        [SerializeField] GameObject selectedDisplay;

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
            SelectFirstAvailable();

            global::Inventory.Instance.onPlantTakeFailed += (id) => selectedDisplay.GetComponent<RectTransform>().DOShakePosition(.5f, 3);

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
        void UpdatePlantsQuantity()
        {
            foreach (var plant in Inventory.Instance.plants)
            {
                _plantsUI[plant.Key].quantity.text = plant.Value.ToString();
            }
        }
        void UpdatePlantQuantity(string id)
        {

        }
        void UpdatePlantQuantity(string id, int quantity)
        {

        }
        void SelectFirstAvailable()
        {
            foreach (var plant in plants)
            {
                if (plant.Value > 0)
                {
                    Select(plant.Key);
                }
            }
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
        }
        public void Hide()
        {
            _tweenTransform.DOAnchorPos3DY(_tweenStartPositionY, _tweenDuration);
        }

        public void Select(string id)
        {
            if (id == _selected)
                return;

            if (!String.IsNullOrEmpty(_selected))
                _plantsUI[_selected].OnDeselected();

            _selected = id;

            _plantsUI[_selected].OnSelected();

            if (onSelectedChanged != null)
                onSelectedChanged.Invoke(id);
        }
    }
}