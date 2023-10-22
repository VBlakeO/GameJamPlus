using DG.Tweening;
using UnityEngine;

public class WaterTank : Singleton<WaterTank>
{
    [SerializeField] Transform waterTankLevel;
    [Range(0, 1)][SerializeField] float _waterTankTweenDuration = .5f;

    void Start()
    {
        WaterReservatory waterReservatory = WaterReservatory.Instance;

        waterTankLevel.localScale = new Vector3(1, waterReservatory.GetWaterPercentage()/100, 1);
        waterReservatory.onAmountChanged += (amount) => waterTankLevel.DOScale(new Vector3(1, waterReservatory.GetWaterPercentage(), 1), _waterTankTweenDuration);
    }
}