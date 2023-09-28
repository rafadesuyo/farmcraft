using UnityEngine;

namespace DreamQuiz
{
    [System.Serializable]
    public class NodeConnection
    {
        [SerializeField] private NodeBase node;
        [SerializeField] private NodePath path;

        public NodeBase Node
        {
            get
            {
                return node;
            }
        }

        public NodePath Path
        {
            get
            {
                return path;
            }
        }

        public NodeConnection(NodeBase node, NodePath path)
        {
            this.node = node;
            this.path = path;
        }
    }
}