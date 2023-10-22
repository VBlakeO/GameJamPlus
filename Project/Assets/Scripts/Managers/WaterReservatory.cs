using UnityEngine;

public class WaterReservatory : Singleton<WaterReservatory>
{
    [SerializeField] private float _reservatoryCapacity = 100f;
    [SerializeField] private float _currentWaterAmount = 100f;
    

    public void AddWater(float water)
    {
        _currentWaterAmount = Mathf.Clamp(_currentWaterAmount + water, 0f, _reservatoryCapacity);
    }

    public void RemoveWater(float water)
    {
         _currentWaterAmount = Mathf.Clamp(_currentWaterAmount - water, 0f, _reservatoryCapacity);
         print(GetWaterPercentage() + "Percentage");
    }

    public float CheckWaterAmount()
    {
        return _currentWaterAmount;
    }

    public float GetWaterPercentage()
    {
        return _currentWaterAmount / _reservatoryCapacity * 100;
    }

}
