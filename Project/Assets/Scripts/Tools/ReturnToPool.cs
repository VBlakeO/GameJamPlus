using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public IObjectPool<GameObject> pool;

    private void OnDisable() 
    {
        pool.Release(gameObject);
    }
}
