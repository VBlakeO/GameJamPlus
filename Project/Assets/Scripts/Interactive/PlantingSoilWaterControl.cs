using UnityEngine;

public class PlantingSoilWaterControl : MonoBehaviour, IInteract
{
    [SerializeField] private float _dryingTimeLimit = 1f;
    [SerializeField] private PlantGreenhouse plantGreenhouse = null;

    private float _reservatoryCapacity = 2f;
    public float _currentWaterAmount = 0f;
    public float _remainingTimeToDry = 0f;

    private void Start() 
    {
        _reservatoryCapacity = plantGreenhouse.plantingSoils.Count;
        _remainingTimeToDry = _dryingTimeLimit;

        if(plantGreenhouse)
            plantGreenhouse.OnHarvest += DryWater;
    }

    public void Interact()
    {
        if (!plantGreenhouse.PlantGreenhouseFertilized())
            return;

        if (WaterReservatory.Instance.CheckWaterAmount() <= 0)
            return;

        if (_currentWaterAmount < _reservatoryCapacity) // Verifica se a quantidade de agua atual e menor que o maximo;
        {
            float dif = _reservatoryCapacity - _currentWaterAmount;

            if(dif >= _reservatoryCapacity/3) // Verifica se a diferenca entre a agua atual e o maximo de agua e maior que um terco
            {
                if(WaterReservatory.Instance.CheckWaterAmount() > _reservatoryCapacity/3)
                {
                    _currentWaterAmount += _reservatoryCapacity / 3;
                    WaterReservatory.Instance.RemoveWater(_reservatoryCapacity / 3);
                }
                else
                {
                    _currentWaterAmount += WaterReservatory.Instance.CheckWaterAmount();
                    WaterReservatory.Instance.RemoveWater(WaterReservatory.Instance.CheckWaterAmount());
                }
            }
            else
            {
                if (WaterReservatory.Instance.CheckWaterAmount() > dif)
                {
                    _currentWaterAmount += dif;
                    WaterReservatory.Instance.RemoveWater(dif);
                }
                else
                {
                    _currentWaterAmount += WaterReservatory.Instance.CheckWaterAmount();
                    WaterReservatory.Instance.RemoveWater(WaterReservatory.Instance.CheckWaterAmount());
                }
            }

            _remainingTimeToDry = _dryingTimeLimit;
        }

        print(_currentWaterAmount);
    }

    private void FixedUpdate() 
    {
        if (plantGreenhouse.PlantGreenhouseFertilized())
        {
            if(!plantGreenhouse.HasHealthyPlants())
                return;

            float dryingRate = plantGreenhouse.plantingSoils[0].plant.consumptionSpeed * plantGreenhouse.plantingSoils.Count;

            if (_currentWaterAmount > 0)
                _currentWaterAmount = Mathf.Clamp(_currentWaterAmount - Time.deltaTime * dryingRate, 0f, _reservatoryCapacity);
            else if(_remainingTimeToDry > 0)
                _remainingTimeToDry = Mathf.Clamp(_remainingTimeToDry - Time.deltaTime * dryingRate, 0f, _reservatoryCapacity);
            else
                DryPlants();
        }

    }

    private void DryPlants()
    {
        if(plantGreenhouse.HasHealthyPlants())
        {
            plantGreenhouse.DryPlants();
            _remainingTimeToDry = _dryingTimeLimit;
        }
    }

    public float GetWaterPercentage()
    {
        return _currentWaterAmount / _reservatoryCapacity * 100;
    }

    private void DryWater()
    {
        _currentWaterAmount = 0f;
        _remainingTimeToDry = _dryingTimeLimit;
        print("DryWater " + _currentWaterAmount);
    }
}
