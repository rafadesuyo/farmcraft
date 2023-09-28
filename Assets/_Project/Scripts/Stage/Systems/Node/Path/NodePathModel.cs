using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    public class NodePathModel : MonoBehaviour
    {
        [SerializeField] private NodePath nodePath;
        [SerializeField] private Transform modelContainer;
        [Header("Options")]
        [SerializeField] private NodePathTilePrefabDatabaseSO nodePathTilePrefabDatabaseSO;
        [SerializeField] private NodePathTilePrefabDatabaseSO.PathTileType tileType;
        [SerializeField] [Range(1, 20)] private int tileCount = 5;
        [SerializeField] [Range(0, 1)] private float minPathLimit = 0.1f;
        [SerializeField] [Range(0, 1)] private float maxPathLimit = 0.9f;

        private GameObject[] tilePrefabs;

        public NodePath NodePath
        {
            get
            {
                return nodePath;
            }
        }

        public Transform ModelContainer
        {
            get
            {
                return modelContainer;
            }
        }

        public int TileCount
        {
            get
            {
                return tileCount;
            }
        }

        public void UpdatePrefabs()
        {
            tilePrefabs = nodePathTilePrefabDatabaseSO.GetPathTilePrefabs(tileType);
        }

        public GameObject GetRandomTile(GameObject previous = null)
        {
            if (tilePrefabs == null || tilePrefabs.Length == 0)
            {
                UpdatePrefabs();
            }

            GameObject prefab;
            prefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];

            if (prefab == previous && tilePrefabs.Length > 2)
            {
                return GetRandomTile(prefab);
            }

            return prefab;
        }

        public Vector3 GetPointAtTime(float t)
        {
            float timeLerp = Mathf.Lerp(minPathLimit, maxPathLimit, t);
            return NodePath.PathCreator.path.GetPointAtTime(timeLerp, PathCreation.EndOfPathInstruction.Stop);
        }

        public Quaternion GetRotationAtTime(float t)
        {
            float timeLerp = Mathf.Lerp(minPathLimit, maxPathLimit, t);
            return NodePath.PathCreator.path.GetRotation(timeLerp, PathCreation.EndOfPathInstruction.Stop);
        }
    }
}