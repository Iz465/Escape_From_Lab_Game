using UnityEngine;
using System.Collections.Generic;

#pragma warning disable // annoying warning keeps coming up that doesnt matter.

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public GameObject prefab;
        public int initialSize = 10;
    }

    private Rigidbody objectBody;
    public List<PoolItem> poolItems = new List<PoolItem>();

    // dict for all prefabs that can be active.
    private Dictionary<GameObject, Queue<GameObject>> poolAvailable = new();
    // Dict for the objects that are active
    private Dictionary<GameObject, HashSet<GameObject>> poolActive = new();

    void Awake()
    {
        foreach (var item in poolItems)
        {
            var availableQueue = new Queue<GameObject>();
            var activeSet = new HashSet<GameObject>();

          
            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                availableQueue.Enqueue(obj);
            }

            poolAvailable[item.prefab] = availableQueue;
            poolActive[item.prefab] = activeSet;
        }
    }

    // Activates prefabs
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolAvailable.ContainsKey(prefab))
        {
            Debug.LogWarning($"{prefab.name} prefab not found!");
            return null;
        }

        if (poolAvailable[prefab].Count <= 0) return null;

        GameObject obj = poolAvailable[prefab].Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        poolActive[prefab].Add(obj);
        return obj;
    }

  // Deactivates prefabs.
    public void ReleaseToPool(GameObject prefab, GameObject obj)
    {
        if (!poolActive.ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool for {prefab.name} unavailable");
            Destroy(obj); // Not from pool
            return;
        }

        if (!poolActive[prefab].Contains(obj))
        {
        //    Debug.LogWarning($"{prefab.name} is not in pool");
            return;
        }

        if (obj != null)
        {
            obj.SetActive(false);
            poolActive[prefab].Remove(obj);
            poolAvailable[prefab].Enqueue(obj);
        }
    }
}
