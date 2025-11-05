using UnityEngine;
using System.Collections.Generic;

#pragma warning disable

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public GameObject prefab;
        public int initialSize = 10;
    }

    public List<PoolItem> poolItems = new List<PoolItem>();

    // all objects in the pool.
    private Dictionary<GameObject, Queue<GameObject>> poolAvailable = new();
    // dict of the current powers
    private Dictionary<GameObject, HashSet<GameObject>> poolActive = new();
    // dict of the prefabs
    private Dictionary<GameObject, GameObject> objectToPrefab = new();

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
                objectToPrefab[obj] = item.prefab; // Track which prefab the instance is.
            }

            poolAvailable[item.prefab] = availableQueue;
            poolActive[item.prefab] = activeSet;
        }
    }


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
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        poolActive[prefab].Add(obj);
        return obj;
    }

    public void ReleaseToPool(GameObject obj)
    {
        if (obj == null) return;

        
        if (!objectToPrefab.TryGetValue(obj, out var prefab))
        {
            Debug.LogWarning($"Object {obj.name} is not from any pool");
            Destroy(obj); // Not from pool
            return;
        }

        if (!poolActive[prefab].Contains(obj)) return;

        obj.SetActive(false);
        poolActive[prefab].Remove(obj);
        poolAvailable[prefab].Enqueue(obj);
    }
}
