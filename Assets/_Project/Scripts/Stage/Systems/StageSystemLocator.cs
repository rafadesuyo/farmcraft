using DreamQuiz;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSystemLocator : MonoBehaviour
{
    private static readonly Dictionary<Type, object> systems = new Dictionary<Type, object>();

    public static void InitializeSystems()
    {
        foreach (var systemObject in systems.Values)
        {
            var system = systemObject as BaseStageSystem;
            system.Initialize();
        }
    }

    public static bool RegisterSystem<T>(T system)
    {
        Type systemType = typeof(T);

        if (IsSystemRegistered<T>())
        {
            return false;
        }

        systems.Add(systemType, system);
        return true;
    }

    public static void UnregisterSystem<T>()
    {
        Type systemType = typeof(T);
        systems.Remove(systemType);
    }

    public static bool IsSystemRegistered<T>()
    {
        Type systemType = typeof(T);
        return systems.ContainsKey(systemType);
    }

    public static T GetSystem<T>()
    {
        Type systemType = typeof(T);

        if (IsSystemRegistered<T>())
        {
            return (T)systems[systemType];
        }

        return default(T);
    }

    public static bool TryGetSystem<T>(out T system)
    {
        Type systemType = typeof(T);

        if (IsSystemRegistered<T>())
        {
            system = (T)systems[systemType];
            return true;
        }

        system = default(T);
        return false;
    }
}
