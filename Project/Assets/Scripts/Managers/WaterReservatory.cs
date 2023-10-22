using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WaterReservatory : Singleton<WaterReservatory>
{
    [SerializeField] private float _reservatoryCapacity = 100f;
    [SerializeField] private float _currentWaterAmount = 100f;

    public Action<float> onAmountChanged;

    protected override void Awake()
    {
        base.Awake();
        _currentWaterAmount = Mathf.Clamp(_currentWaterAmount, 0f, _reservatoryCapacity);
    }

    public void AddWater(float water)
    {
        _currentWaterAmount = Mathf.Clamp(_currentWaterAmount + water, 0f, _reservatoryCapacity);

        if (onAmountChanged != null)
            onAmountChanged.Invoke(_currentWaterAmount);
    }

    public void RemoveWater(float water)
    {
         _currentWaterAmount = Mathf.Clamp(_currentWaterAmount - water, 0f, _reservatoryCapacity);

        if (onAmountChanged != null)
            onAmountChanged.Invoke(_currentWaterAmount);

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