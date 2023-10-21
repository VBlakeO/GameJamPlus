using UnityEngine.Pool;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{   
    private bool collectionChecks = true;
    [SerializeField] private int maxPoolSize = 12;

    public string id = "";
    public GameObject plant;
    IObjectPool<GameObject> m_Pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_Pool == null)
                m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            return m_Pool;
        }
    }

    GameObject CreatePooledItem()
    {
        var go = Instantiate(plant);
        go.SetActive(false);
        go.transform.parent = transform;

        // This is used to return ParticleSystems to the pool when they have stopped.
        var returnToPool = go.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;

        return go;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
}
