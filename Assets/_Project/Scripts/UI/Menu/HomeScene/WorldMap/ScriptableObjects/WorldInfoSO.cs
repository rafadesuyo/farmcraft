using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Map/World Info")]
public class WorldInfoSO : ScriptableObject
{
    //Variables
    [Header("Variables")]
    [SerializeField] private string worldName;
    [SerializeField] private GameObject worldPrefab;

    //Getters
    public string WorldName => worldName;
    public GameObject WorldPrefab => worldPrefab;
}
