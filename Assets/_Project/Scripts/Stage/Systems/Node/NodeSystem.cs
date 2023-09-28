using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz
{
    public class NodeSystem : BaseStageSystem
    {
        public static event Action<NodeSystem> OnNodeSystemStart;
        public static event Action<NodeSystem> OnNodeSystemInitialize;
        public enum NodeType
        {
            Default,
            EndOfStage,
            Wall,
            Door
        }

        public enum PawnType
        {
            Sheep,
            Wolf,
            Death,
            Antidote,
            Poison,
            Armor,
            SleepingTime,
            Penalty,
            Gold,
            Key,
            Question,
            Power
        }

        public enum PathType
        {
            Default
        }

        [SerializeField] private NodeElementsPrefabDatabaseSO nodeElementsPrefabDatabase;
        private bool isRegistered = false;
        private List<NodeBase> stageNodes = new List<NodeBase>();
        private Queue<NodeBase> entranceNodes = new Queue<NodeBase>();
        public Pawn CurrentPawn { get; private set; }
        public bool HasPawn
        {
            get
            {
                return CurrentPawn != null;
            }
        }

        public IReadOnlyList<NodeBase> StageNodes
        {
            get
            {
                return stageNodes.AsReadOnly();
            }
        }

        public NodeElementsPrefabDatabaseSO NodeElementsPrefabDatabase
        {
            get
            {
                return nodeElementsPrefabDatabase;
            }
        }

        private void Start()
        {
            if (isRegistered == false)
            {
                Debug.LogWarning($"[NodeSystem] System not registered to start");
                return;
            }

            OnNodeSystemStart?.Invoke(this);
        }

        public override void Initialize()
        {
            if (isRegistered == false)
            {
                Debug.LogWarning($"[NodeSystem] System not registered to initialize");
                return;
            }

            OnNodeSystemInitialize?.Invoke(this);

            IsReady = true;
        }

        public void RegisterNode(NodeBase nodeBase)
        {
            if (nodeBase == null)
            {
                Debug.LogWarning($"[NodeSystem] null NodaBase trying to be added");
                return;
            }

            if (stageNodes.Contains(nodeBase))
            {
                Debug.LogWarning($"[NodeSystem] Trying to register {nodeBase.name} but it is already registered");
                return;
            }

            stageNodes.Add(nodeBase);
        }

        public void UnregisterNode(NodeBase nodeBase)
        {
            stageNodes.Remove(nodeBase);
        }

        public NodeBase GetNextEntranceNode()
        {
            if (entranceNodes.Count == 0)
            {
                entranceNodes = new Queue<NodeBase>(stageNodes.Where(n => n.IsEntranceNode));
            }

            if (entranceNodes.Count == 0)
            {
                Debug.LogError("Missing entrance node");
                return stageNodes[0]
                    ?? default;
            }

            return entranceNodes.Dequeue();
        }

        public void SetCurrentPawn(Pawn pawn)
        {
            CurrentPawn = pawn;
        }

        public bool SpawnPawnToNode(NodeBase nodeBase, PawnType prefabType)
        {
            var pawns = nodeBase.GetNonPlayerPawnsInNode();

            if (pawns.Length > 1)
            {
                return false;
            }

            var prefab = nodeElementsPrefabDatabase.GetPawnPrefabByType(prefabType);

            if (prefab == null)
            {
                Debug.LogError($"[NodeSystem] Prefab for type {prefabType} not found in database");
                return false;
            }

            var pawn = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            nodeBase.AddPawnToNode(pawn);
            return true;
        }

        public NodeBase SpawnNodeBase(NodeType nodeType)
        {
            Transform pathContainer = transform.Find("Paths");
            var prefab = nodeElementsPrefabDatabase.GetNodeBasePrefabByType(nodeType);
            return Instantiate(prefab, pathContainer);
        }

        public NodeBase SwapNodeBase(NodeBase swapNodeBase, NodeType swapToNodeType)
        {
            var newNodeBase = SpawnNodeBase(swapToNodeType);
            newNodeBase.transform.position = swapNodeBase.transform.position;

            foreach (var connection in swapNodeBase.Connections)
            {
                newNodeBase.SetConnection(connection);
                NodeHelper.CreateInverseConnection(newNodeBase, connection);
            }

            Destroy(swapNodeBase.gameObject);

            return newNodeBase;
        }

        public void TriggerInteractionWithNode(NodeBase node)
        {
            if (IsReady == false)
            {
                Debug.Log($"[NodeSystem] System is not ready, movement to node {gameObject.name} failed");
                return;
            }

            if (HasPawn == false)
            {
                Debug.Log($"[NodeSystem] System doesn't have a registered pawn, interaction with node {gameObject.name} failed");
                return;
            }

            if (node.CanInteractWithPawn(CurrentPawn, out string message) == false)
            {
                Debug.Log($"[NodeSystem] {message}");
                return;
            }

            CurrentPawn.InteractWithNode(node);
        }

        protected override void RegisterSystem()
        {
            isRegistered = StageSystemLocator.RegisterSystem(this);
        }

        protected override void UnregisterSystem()
        {
            StageSystemLocator.UnregisterSystem<NodeSystem>();
        }
    }
}