using DreamQuiz.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{

    public class NodeBase : MonoBehaviour, ITargetable
    {
        [Header("Options")]
        [SerializeField] protected bool isEntranceNode = false;
        [SerializeField] protected bool onlyPlayerCanStay = false;
        [SerializeField] protected List<NodeConnection> connections = new List<NodeConnection>();

        [Header("Pawn")]
        [SerializeField] protected Transform pawnContainer = null;
        [SerializeField] protected float rearrengementDelta = 0.5f;

        [Header("Model")]
        [SerializeField] protected NodeBaseModel nodeBaseModel = null;

        protected NodeSystem nodeSystem = null;
        protected NodeBaseModelType currentNodeBaseModelType = NodeBaseModelType.DefaultModel;
        protected bool isInitialized = false;
        protected bool isBlocked = false;

        public event Action<Pawn> OnPawnAdded;

        public bool IsEntranceNode
        {
            get
            {
                return isEntranceNode;
            }
        }
        public bool OnlyPlayerCanStay
        {
            get
            {
                return onlyPlayerCanStay;
            }
        }

        public IReadOnlyList<NodeConnection> Connections
        {
            get
            {
                return connections.AsReadOnly();
            }
        }

        public bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        public bool IsBlocked
        {
            get
            {
                return isBlocked;
            }
        }

        public Transform PawnContainer
        {
            get
            {
                return pawnContainer;
            }
        }

        public NodeBaseModel NodeBaseModel
        {
            get
            {
                return nodeBaseModel;
            }
        }

        protected virtual void Awake()
        {
            NodeSystem.OnNodeSystemStart += NodeSystem_OnNodeSystemStart;
            NodeSystem.OnNodeSystemInitialize += NodeSystem_OnNodeSystemInitialize;

            nodeBaseModel.UpdateNodeModel(GetNodeBaseModelSkinType());
            nodeBaseModel.PlayAnimation(NodeBaseModel.NodeAnimation.Idle);

            foreach (var pawn in pawnContainer.GetComponentsInChildren<Pawn>())
            {
                pawn.SetPawnToNode(this);
            }
        }

        public void UpdateName()
        {
            name = GetNodeName();
        }

        protected virtual string GetNodeName()
        {
            return $"NodeBase_Default_{transform.GetSiblingIndex()}";
        }

        public virtual string GetPawnDescription()
        {
            var pawns = GetNonPlayerPawnsInNode();
            string description = "";

            foreach (var pawn in pawns)
            {
                string descr = pawn.GetDescription();

                if (string.IsNullOrEmpty(descr) == false)
                {
                    description += descr;
                }
            }

            return description;
        }

        public virtual bool CanInteractWithPawn(Pawn pawn, out string message)
        {
            message = string.Empty;
            return pawn != null;
        }

        public virtual string GetNodeDescription()
        {
            return "Just a common node.";
        }

        private void NodeSystem_OnNodeSystemStart(NodeSystem nodeSystem)
        {
            NodeSystem.OnNodeSystemInitialize -= NodeSystem_OnNodeSystemStart;

            this.nodeSystem = nodeSystem;
            nodeSystem.RegisterNode(this);
        }

        private void NodeSystem_OnNodeSystemInitialize(NodeSystem obj)
        {
            isInitialized = true;
        }

        public virtual void AddPawnToNode(Pawn pawn)
        {
            pawn.transform.SetParent(pawnContainer);

            RearrangePawns();

            OnPawnAdded?.Invoke(pawn);
        }

        public virtual void PawnExited(Pawn pawn)
        {
            pawn.transform.parent = null;

            RearrangePawns();
        }

        public void RearrangePawns()
        {
            int childCount = pawnContainer.childCount;

            if (childCount == 1)
            {
                var child = pawnContainer.GetChild(0);
                child.localPosition = Vector3.zero;
                return;
            }

            for (int i = 0; i < childCount; i++)
            {
                float t = 0;

                if (i > 0)
                {
                    t = (float)i / (childCount - 1);
                }

                Vector3 newPosition = Vector3.Lerp(Vector3.left * rearrengementDelta, Vector3.right * rearrengementDelta, t);
                var child = pawnContainer.GetChild(i);
                child.localPosition = newPosition;
            }
        }

        public void TriggerNodeInteraction()
        {
            if (isInitialized == false)
            {
                Debug.Log($"Node {gameObject.name} not initialized");
                return;
            }

            nodeSystem.TriggerInteractionWithNode(this);
        }

        public void SetBlock(bool value)
        {
            isBlocked = value;
        }

        public void SetConnection(NodeConnection nodeConnection)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (nodeConnection.Node == connections[i].Node
                    || nodeConnection.Path == connections[i].Path)
                {
                    connections[i] = nodeConnection;
                    return;
                }
            }

            connections.Add(nodeConnection);
        }

        public void UpdateConnectionAt(NodeConnection nodeConnection, int index)
        {
            if (index < 0 || index >= connections.Count)
            {
                Debug.LogError($"[NodeBase] Unable to update connection at index {index} was out of range");
                return;
            }

            connections[index] = nodeConnection;
        }

        public Pawn[] GetPawnsInNode()
        {
            return PawnContainer.GetComponentsInChildren<Pawn>();
        }

        public Pawn[] GetNonPlayerPawnsInNode()
        {
            return PawnContainer.GetComponentsInChildren<Pawn>().Where(p => (p is PlayerPawn) == false).ToArray();
        }

        public void SetNodeBaseModelType(NodeBaseModelType nodeBaseModelType)
        {
            currentNodeBaseModelType = nodeBaseModelType;
            nodeBaseModel.UpdateNodeModel(GetNodeBaseModelSkinType());
        }

        public virtual NodeBaseModelType GetNodeBaseModelSkinType()
        {
            return currentNodeBaseModelType;
        }

        public void TargetSelected()
        {
            Debug.Log($"{gameObject.name} was targeted");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 20;

            foreach (var connection in connections)
            {
                if (connection.Path != null)
                {
                    for (int i = 0; i <= 20; i++)
                    {
                        Gizmos.color = Color.white;
                        Vector3 position = connection.Path.PathCreator.path.GetPointAtTime(i * 0.05f, PathCreation.EndOfPathInstruction.Stop);
                        Gizmos.DrawSphere(position, 0.1f);

                        if (i == 10)
                        {
                            Handles.Label(position + Vector3.right * 0.4f, connection.Path.name, style);
                        }
                    }
                }

                if (connection.Node != null)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(connection.Node.transform.position, 0.4f);
                    Handles.Label(connection.Node.transform.position + Vector3.right * 0.6f, connection.Node.name, style);
                }
            }

            style.normal.textColor = Color.green;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.4f);
            Handles.Label(transform.position + Vector3.right * 0.6f, name, style);
        }
#endif
    }
}