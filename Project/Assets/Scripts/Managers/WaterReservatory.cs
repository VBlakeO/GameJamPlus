using System;
using UnityEngine;
using DG.Tweening;


public class WaterReservatory : Singleton<WaterReservatory>
{
    [SerializeField] private float _reservatoryCapacity = 100f;
    [SerializeField] private float _currentWaterAmount = 100f;



    [SerializeField] Transform waterTankLevel;
    [Range(0, 1)][SerializeField] float _waterTankTweenDuration = .5f;

    public Action<float> onAmountChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        waterTankLevel.localScale = new Vector3(1, GetWaterPercentage(), 1);
    }

    public void AddWater(float water)
    {
        _currentWaterAmount = Mathf.Clamp(_currentWaterAmount + water, 0f, _reservatoryCapacity);

        waterTankLevel.DOScale(new Vector3(1, GetWaterPercentage(), 1), _waterTankTweenDuration);

        if (onAmountChanged != null)
            onAmountChanged.Invoke(_currentWaterAmount);
    }

    public void RemoveWater(float water)
    {
         _currentWaterAmount = Mathf.Clamp(_currentWaterAmount - water, 0f, _reservatoryCapacity);

        waterTankLevel.DOScale(new Vector3(1, GetWaterPercentage(), 1), _waterTankTweenDuration);

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