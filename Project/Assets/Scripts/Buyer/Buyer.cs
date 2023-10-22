using UnityEngine;
using DG.Tweening;


public class Buyer : Singleton<Buyer>
{
    [SerializeField] Transform _tweenTransform;
    [SerializeField] Transform _tweenTargetPositionCatchPoint;
    [SerializeField] Transform _tweenTargetPositionOffPoint;

    [SerializeField] bool collectTimeoutIsRunning = true;

    [Range(0, 1)][SerializeField] float _collectTimeout = 1;
    public float _collectTimeInSeconds = 5;

    void Update()
    {
        TickTimeout(Time.deltaTime);
    }

    void TickTimeout(float deltaTime)
    {
        if (!collectTimeoutIsRunning)
            return;

        _collectTimeout = Mathf.Clamp01(_collectTimeout - (deltaTime / _collectTimeInSeconds));

        if (_collectTimeout == 0)
        {
            GetSilosContent();
            _collectTimeout = 1;
        }
    }

    public void BuyAll()
    {
        foreach (var silo in SilosManager.Instance.silos)
        {
            Buy(silo);
        }
    }

    public void Buy(Silo silo)
    {
        int siloPrice = PlantStaticsHolder.Instance.plantStatics[silo.id].price * silo.quantity;
        silo.ResetPlant();
        PlayerCurrency.Instance.Add(siloPrice);
    }

    public void GetSilosContent()
    {
        _tweenTransform.localScale = Vector3.zero;
        _tweenTransform.position = _tweenTargetPositionCatchPoint.position;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_tweenTransform.DOScale(Vector3.one, .5f));
        sequence.AppendInterval(1);
        sequence.onComplete += BuyAll;
    }

    public void MoveOffSilos()
    {
        _tweenTransform.localScale = Vector3.zero;
        _tweenTransform.position = _tweenTargetPositionCatchPoint.position;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.Append(_tweenTransform.DOMove(_tweenTargetPositionOffPoint.position, 2));
        sequence.Append(_tweenTransform.DOScale(Vector3.zero, 2));
    }
}