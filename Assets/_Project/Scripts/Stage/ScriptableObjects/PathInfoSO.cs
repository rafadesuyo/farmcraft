using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Path Info")]

public class PathInfoSO : ScriptableObject
{
    //Variables
    [Header("Variables")]
    [SerializeField] private int sleepingTimeToTraverse = 1;

    [Header("Options")]
    [SerializeField] private Material pathMaterial;
    [SerializeField] private Material undersideMaterial;

    [Space(10)]

    [SerializeField] private float textureTilingMultiplier = 1;

    //Getters
    public int SleepingTimeToTraverse => sleepingTimeToTraverse;
    public Material PathMaterial => pathMaterial;
    public Material UndersideMaterial => undersideMaterial;
    public float TextureTilingMultiplier => textureTilingMultiplier;
}
