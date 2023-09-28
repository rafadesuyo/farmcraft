using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class Pool<T>
{
    public ObjectPool<Component> items;

    public void SetupPool(Func<Component> onCreate, Action<Component> onGet, Action<Component> onRelease, int poolCapacity, int poolMaxSize)
    {
        items = new ObjectPool<Component>
        (
            onCreate, onGet, onRelease, null, false, poolCapacity, poolMaxSize
        );
    }

    public void ClearPool()
    {
        items.Dispose();
    }
}

public static class GenericPool
{
    public static readonly int POOL_DEFAULT_CAPACITY = 10;
    // Arbitrary number, but based on game needs
    public static readonly int POOL_MAX_SIZE = 100;

    private static Dictionary<string, Pool<Component>> pools = new Dictionary<string, Pool<Component>>();

#if UNITY_EDITOR
    private static bool SHOW_DEBUGS = false;
#endif

    // Currently, only one pool is shared between parents. This id behavior can avoid it,
    // allowing multiple pools per type.
    public static void CreatePool<T>(GameObject prefab, Transform parent, string id = "") where T : Component
    {
        TryCreatePool<T>(prefab, parent, id);
    }

    public static void ClearPools()
    {
        foreach (var pool in pools)
        {
            pool.Value.ClearPool();
        }

        pools.Clear();
    }

    public static T GetItem<T>(string poolId = "") where T : Component
    {
        return GetPoolForItemById(typeof(T), poolId).items.Get() as T;
    }

    public static void ReleaseItem(Type type, Component item, string poolId = "")
    {
        GetPoolForItemById(type, poolId)?.items.Release(item);
    }

    private static Component OnCreate<T>(GameObject prefab, Transform parent)
    {
        return Component.Instantiate(prefab, parent).GetComponent(typeof(T));
    }

    private static void OnGet(Component item)
    {
        item.gameObject.SetActive(true);
        item.transform.SetAsLastSibling();
    }

    private static void OnRelease(Component c)
    {
        c.gameObject.SetActive(false);
    }

    private static void TryCreatePool<T>(GameObject prefab, Transform parent, string id)
    {
        Pool<Component> pool = GetPoolForItemById(typeof(T), id);

        if (pool == null)
        {
            pool = new Pool<Component>();
            pool.SetupPool(() =>
            {
                return OnCreate<T>(prefab, parent);
            },
            OnGet, OnRelease, POOL_DEFAULT_CAPACITY, POOL_MAX_SIZE);

            pools.Add(GetFormattedId(typeof(T), id), pool);

#if UNITY_EDITOR
            if (SHOW_DEBUGS)
            {
                Debug.Log($"New pool has been created. Id: {GetFormattedId(typeof(T), id)}.");
            }
#endif
        }

#if UNITY_EDITOR
        else
        {
            if (SHOW_DEBUGS)
            {
                Debug.Log($"A pool with the id {GetFormattedId(typeof(T), id)} already existis. Skpping.");
            }
        }
#endif
    }

    private static Pool<Component> GetPoolForItemById(Type type, string id = "")
    {
        string formattedId = GetFormattedId(type, id);

        if (!pools.ContainsKey(formattedId))
        {
#if UNITY_EDITOR
            if (SHOW_DEBUGS)
            {
                Debug.Log($"There is no pool called {formattedId}. Fix it. Pools can't be null.");
            }
#endif
            return null;
        }
        else
        {
#if UNITY_EDITOR
            if (SHOW_DEBUGS)
            {
                Debug.Log($"Calling pool with id: {formattedId}.");
            }
#endif
            return pools[formattedId];
        }
    }

    private static string GetFormattedId(Type type, string id = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            id = $"{type}";
        }

        return id;
    }
}
