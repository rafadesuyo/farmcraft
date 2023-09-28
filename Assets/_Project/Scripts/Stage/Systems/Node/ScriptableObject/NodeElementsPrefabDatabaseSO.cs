using UnityEngine;
using System.Linq;

namespace DreamQuiz
{
    [CreateAssetMenu(fileName = "NewNodeElementsPrefabDatabaseSO", menuName = "Node/Node Elements Prefab Database")]
    public class NodeElementsPrefabDatabaseSO : ScriptableObject
    {
        [System.Serializable]
        public class NodeBasePrefab
        {
            public NodeSystem.NodeType PrefabType;
            public NodeBase Prefab;
        }

        [System.Serializable]
        public class PawnPrefab
        {
            public NodeSystem.PawnType PrefabType;
            public Pawn Prefab;
        }

        [System.Serializable]
        public class PathPrefab
        {
            public NodeSystem.PathType PrefabType;
            public NodePath Prefab;
        }

        [SerializeField] private NodeBasePrefab[] nodeBasePrefabs;
        [SerializeField] private PawnPrefab[] pawnPrefabs;
        [SerializeField] private PathPrefab[] pathPrefabs;

        public NodeBase GetNodeBasePrefabByType(NodeSystem.NodeType prefabType)
        {
            NodeBase prefab = null;

            if (nodeBasePrefabs != null && nodeBasePrefabs.Length > 0)
            {
                var element = nodeBasePrefabs.First(el => el.PrefabType == prefabType);

                if (element == null)
                {
                    Debug.LogError($"[NodeElementsPrefabDatabaseSO] Prefab for node type {prefabType} not found");
                    return null;
                }

                prefab = element.Prefab;
            }

            return prefab;
        }

        public Pawn GetPawnPrefabByType(NodeSystem.PawnType prefabType)
        {
            Pawn prefab = null;

            if (pawnPrefabs != null && pawnPrefabs.Length > 0)
            {
                var element = pawnPrefabs.First(el => el.PrefabType == prefabType);

                if (element == null)
                {
                    Debug.LogError($"[NodeElementsPrefabDatabaseSO] Prefab for node type {prefabType} not found");
                    return null;
                }

                prefab = element.Prefab;
            }

            return prefab;
        }

        public NodePath GetPathPrefabByType(NodeSystem.PathType prefabType)
        {
            NodePath prefab = null;

            if (pathPrefabs != null && pathPrefabs.Length > 0)
            {
                var element = pathPrefabs.First(el => el.PrefabType == prefabType);

                if (element == null)
                {
                    Debug.LogError($"[NodeElementsPrefabDatabaseSO] Prefab for node type {prefabType} not found");
                    return null;
                }

                prefab = element.Prefab;
            }

            return prefab;
        }
    }
}