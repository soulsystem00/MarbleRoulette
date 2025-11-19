using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PoolType
{
    None,
    Player,

}

[System.Serializable]
public class ObjectPool
{
    public PoolType poolType;
    public GameObject prefab;
    public int poolSize;
    public int maxPoolSize;
}

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    private const int MaxDestroyPerFrame = 10;
    private const float DestroyInterval = 0.5f;

    [SerializeField] List<ObjectPool> objectPools = new List<ObjectPool>();

    private Dictionary<PoolType, ObjectPool> poolInfoDictionary = new Dictionary<PoolType, ObjectPool>();
    private Dictionary<PoolType, Queue<GameObject>> poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
    private Queue<GameObject> destroyQueue = new Queue<GameObject>();
    private WaitForSeconds destoryInterval;

    protected override void Init()
    {
        destoryInterval = new WaitForSeconds(DestroyInterval);

        if (poolDictionary == null)
        {
            poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
        }

        if (destroyQueue == null)
        {
            destroyQueue = new Queue<GameObject>();
        }

        foreach (var pool in objectPools)
        {
            if (pool.poolType == PoolType.None)
            {
                Debug.LogError("PoolType.None is not allowed for object pools.");
                continue;
            }

            poolDictionary[pool.poolType] = new Queue<GameObject>();
            poolInfoDictionary[pool.poolType] = pool;
            for (int i = 0; i < pool.poolSize; i++)
            {
                var go = Instantiate(pool.prefab, this.transform);

                go.SetActive(false);
                poolDictionary[pool.poolType].Enqueue(go);
            }
        }

        StartCoroutine(ProcessPendingDestroy());
    }

    private IEnumerator ProcessPendingDestroy()
    {
        while (true)
        {
            int destroyCount = Mathf.Min(MaxDestroyPerFrame, destroyQueue.Count);

            for (int i = 0; i < destroyCount; i++)
            {
                var go = destroyQueue.Dequeue();
                Destroy(go);
            }

            yield return destoryInterval;
        }
    }

    public T Instantiate<T>(PoolType poolType, Vector3 position, Quaternion rotation, Transform parents = null) where T : MonoBehaviour
    {
        if (poolDictionary.TryGetValue(poolType, out var pool))
        {
            if (pool.Count > 0)
            {
                var go = pool.Dequeue();
                go.SetActive(true);
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.transform.parent = parents;

                var comp = go.GetComponent<T>();

                if (comp == null)
                {
                    Debug.LogError($"The component of type {typeof(T)} does not exist on the prefab for pool type: {poolType}");
                    return null;
                }

                return comp;
            }
            else
            {
                if (poolInfoDictionary.TryGetValue(poolType, out var poolInfo))
                {
                    for (int i = 0; i < poolInfo.poolSize; i++)
                    {
                        var go2 = Instantiate(poolInfo.prefab, this.transform);
                        go2.SetActive(false);
                        pool.Enqueue(go2);
                    }

                    var go = pool.Dequeue();
                    go.SetActive(true);
                    go.transform.position = position;
                    go.transform.rotation = rotation;
                    go.transform.parent = parents;

                    var comp = go.GetComponent<T>();

                    if (comp == null)
                    {
                        Debug.LogError($"The component of type {typeof(T)} does not exist on the prefab for pool type: {poolType}");
                        return null;
                    }

                    return comp;
                }
                else
                {
                    Debug.LogError($"No pool info found for pool type: {poolType}");
                    return null;
                }
            }
        }

        return null;
    }

    public void Release(PoolType poolType, GameObject go, bool forceRelease = false)
    {
        if (poolDictionary.TryGetValue(poolType, out var pool))
        {
            if (poolInfoDictionary.TryGetValue(poolType, out var poolInfo))
            {
                go.SetActive(false);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.parent = this.transform;

                if (pool.Count >= poolInfo.maxPoolSize)
                {
                    if (forceRelease == true)
                    {
                        Destroy(go);
                    }
                    else
                    {
                        destroyQueue.Enqueue(go);
                    }
                }
                else
                {
                    pool.Enqueue(go);
                }
            }
            else
            {
                Debug.LogError($"No pool info found for pool type: {poolType}");
                return;
            }
        }
        else
        {
            Debug.LogError($"No pool found for pool type: {poolType}");
        }
    }
}
