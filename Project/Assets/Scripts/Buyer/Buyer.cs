using UnityEngine;
using DG.Tweening;


public class Buyer : Singleton<Buyer>
{
    [SerializeField] Transform truckTransform;
    [SerializeField] Transform _tweenTargetPositionCatchPoint;
    [SerializeField] Transform _tweenTargetPositionOffPoint;

    [SerializeField] bool collectTimeoutIsRunning = true;

    [Range(0, 1)][SerializeField] float _collectTimeout = 1;
    public float _collectTimeInSeconds = 5;

    protected override void Awake()
    {
        base.Awake();
        truckTransform.localScale = Vector3.zero;
        truckTransform.position = _tweenTargetPositionCatchPoint.position;
    }

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
        if (silo.id == null)
            return;

        if (silo.quantity <= 0)
            return;

        int siloPrice = PlantStaticsHolder.Instance.plantStatics[silo.id].sellPrice * silo.quantity;
        silo.ResetPlant();
        PlayerCurrency.Instance.Add(siloPrice);
    }

    public void GetSilosContent()
    {
        truckTransform.localScale = Vector3.zero;
        truckTransform.position = _tweenTargetPositionCatchPoint.position;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(truckTransform.DOScale(Vector3.one, .5f));
        sequence.AppendInterval(.5f);

        sequence.onComplete += BuyAll;
        sequence.onComplete += MoveOffSilos;
    }

    public void MoveOffSilos()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.Append(truckTransform.DOScale(Vector3.zero, 1));
        sequence.Append(truckTransform.DOMove(_tweenTargetPositionOffPoint.position, 2));

        sequence.onComplete += () => truckTransform.localScale = Vector3.zero;
        sequence.onComplete += () => truckTransform.position = _tweenTargetPositionCatchPoint.position;
    }
}