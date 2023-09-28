using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    [RequireComponent(typeof(NodeMovement))]
    public abstract class Pawn : MonoBehaviour
    {
        public NodeMovement NodeMovement { get; protected set; }
        public NodeBase CurrentNode { get; protected set; }
        public Stack<NodeBase> NodeHistory { get; protected set; }

        protected virtual void Awake()
        {
            NodeHistory = new Stack<NodeBase>();
            NodeMovement = GetComponent<NodeMovement>();
            NodeMovement.OnArrival += NodeMovement_OnArrival;
        }

        protected virtual void OnEnable()
        {
            NodeBase nodeBase = transform.GetComponentInParent<NodeBase>();

            if (nodeBase != null)
            {
                SetPawnToNode(nodeBase);
            }
        }

        protected virtual void OnDestroy()
        {
            NodeMovement.OnArrival -= NodeMovement_OnArrival;
        }

        private void NodeMovement_OnArrival(NodeBase nodeBase, NodePath nodePath)
        {
            SetPawnToNode(nodeBase);
        }

        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public virtual void SetPawnToNode(NodeBase nodeBase)
        {
            if (CurrentNode != null)
            {
                CurrentNode.PawnExited(this);
                NodeHistory.Push(CurrentNode);
            }

            CurrentNode = nodeBase;
            CurrentNode.AddPawnToNode(this);
        }

        public void ReturnToPreviousNode()
        {
            if (NodeHistory.Count == 0)
            {
                Debug.LogWarning($"Pawn {name} doesn't have a previous node");
                return;
            }

            var previousNode = NodeHistory.Pop();
            NodeMovement.TryMoveToNode(CurrentNode, previousNode);
        }

        public void TeleportToNode(NodeBase nodeBase)
        {
            SetPawnToNode(nodeBase);
            transform.localPosition = Vector3.zero;
        }

        public virtual void InteractWithNode(NodeBase node)
        {
            NodeMovement.TryMoveToNode(CurrentNode, node);
        }

        public void RemovePawnFromBoard()
        {
            transform.SetParent(null);
            CurrentNode?.RearrangePawns();
            Destroy(gameObject);
        }
    }
}