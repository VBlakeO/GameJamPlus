using UnityEngine;

public class Buyer : Singleton<Buyer>
{
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
            BuyAll();
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
}