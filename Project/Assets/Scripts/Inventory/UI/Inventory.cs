using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI.Inventory
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] Transform _plantsHolder;
        [SerializeField] GameObject _plantPrefab;
        Dictionary<string, Plant> _plants;

        [SerializeField] string _selected;
        public string currentPlant => _selected;

        public Action<string> onSelectedChanged;


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
                instance = MonoBehaviour.Instantiate(_plantPrefab, _plantsHolder);

                Plant plant = instance.GetComponent<Plant>();
                plant.Set(item.Key);
                _plants.Add(item.Key, plant);
            }
        }

        public void Select(string id)
        {
            if (!String.IsNullOrEmpty(_selected))
                _plants[_selected].OnDeselected();

            _selected = id;

            _plants[_selected].OnSelected();

            onSelectedChanged.Invoke(id);
        }
    }
}