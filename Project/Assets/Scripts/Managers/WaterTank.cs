using DG.Tweening;
using UnityEngine;

public class WaterTank : Singleton<WaterTank>
{
    [SerializeField] Transform _levelTransform;
    [Range(0, 1)][SerializeField] float levelTweenDuration = .5f;

    void Start()
    {
        WaterReservatory waterReservatory = WaterReservatory.Instance;

        _levelTransform.localScale = new Vector3(1, waterReservatory.GetWaterPercentage()/100, 1);
        waterReservatory.onAmountChanged += (amount) => _levelTransform.DOScale(new Vector3(1, waterReservatory.GetWaterPercentage()/100, 1), levelTweenDuration);
    }
}