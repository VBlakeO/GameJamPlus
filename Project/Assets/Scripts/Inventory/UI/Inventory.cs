using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UI.Inventory
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] bool isOpen = false;
        [Space]
        float tweenStartPositionY;
        public float tweenTargetPositionY;
        [Space]
        [SerializeField] Transform _plantsContent;
        [SerializeField] GameObject _plantPrefab;
        Dictionary<string, Plant> _plants;
        [Space]
        [SerializeField] string _selected;
        public string currentPlant => _selected;
        public Action<string> onSelectedChanged;


        protected override void Awake()
        {
            base.Awake();
            tweenStartPositionY = transform.position.y;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                SwitchState();
        }

        void Start()
        {
            InstantiateItems();
        }

        void InstantiateItems()
        {
            _plants = new Dictionary<string, Plant>();

            GameObject instance = null;

            foreach (KeyValuePair<string, PlantStatic> item in PlantStaticsHolder.Instance.plantStatics)
            {
                instance = MonoBehaviour.Instantiate(_plantPrefab, _plantsContent);

                Plant plant = instance.GetComponent<Plant>();
                plant.Set(item.Key);
                _plants.Add(item.Key, plant);
            }
        }

        public void SetState(bool b)
        {
            if (isOpen == b)
                return;

            SwitchState();
        }
        public void SwitchState()
        {
            isOpen = !isOpen;

            if (isOpen)
                Show();
            else
                Hide();
        }
        public void Show()
        {
            transform.DOMoveY(tweenTargetPositionY, .5f);
        }

        public void Hide()
        {
            transform.DOMoveY(tweenStartPositionY, .5f);
        }

        public void Select(string id)
        {
            if (!String.IsNullOrEmpty(_selected))
                _plants[_selected].OnDeselected();

            _selected = id;

            _plants[_selected].OnSelected();

            if (onSelectedChanged != null)
                onSelectedChanged.Invoke(id);
        }
    }
}