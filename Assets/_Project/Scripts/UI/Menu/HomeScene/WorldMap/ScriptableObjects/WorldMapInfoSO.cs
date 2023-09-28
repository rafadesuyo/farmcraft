using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Map/World Map Info")]
public class WorldMapInfoSO : ScriptableObject
{
    //Variables
    [Header("Variables")]
    [SerializeField] private WorldInfoSO[] worlds;

    //Getters
    public WorldInfoSO[] Worlds => worlds;
}