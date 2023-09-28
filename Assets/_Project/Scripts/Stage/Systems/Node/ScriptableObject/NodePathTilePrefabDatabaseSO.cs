using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz
{
    [CreateAssetMenu(fileName = "NodePathTilePrefabDatabaseSO", menuName = "Node/Path/Node Path Tile Prefab Database")]
    public class NodePathTilePrefabDatabaseSO : ScriptableObject
    {
        public enum PathTileType
        {
            Default
        }

        [System.Serializable]
        public class PathTilePrefab
        {
            public PathTileType PathTileType;
            public GameObject[] Prefabs;
        }

        [SerializeField] private PathTilePrefab[] pathTilePrefabs;

        public GameObject[] GetPathTilePrefabs(PathTileType pathTileType)
        {
            GameObject[] prefabs = new GameObject[0];

            if (pathTilePrefabs != null && pathTilePrefabs.Length > 0)
            {
                var element = pathTilePrefabs.First(el => el.PathTileType == pathTileType);

                if (element == null)
                {
                    Debug.LogError($"[NodePathTilePrefabDatabaseSO] Prefab for path tile type {pathTileType} not found");
                    return null;
                }

                prefabs = element.Prefabs;
            }

            return prefabs;
        }
    }
}