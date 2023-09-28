using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    [CreateAssetMenu(fileName = "NewNodeBaseModelSkinDatabase", menuName = "Node/NodeBase Model Skin Database")]
    public class NodeBaseModelSkinDatabaseSO : ScriptableObject
    {
        [Serializable]
        public class NodeBaseSkin
        {
            public NodeBaseModelType NodeBaseModelType;
            public string skinName;
        }

        [SerializeField] private List<NodeBaseSkin> nodeBaseModels;

        public IReadOnlyList<NodeBaseSkin> NodeBaseModels
        {
            get
            {
                return nodeBaseModels.AsReadOnly();
            }
        }

        public string GetSkinByType(NodeBaseModelType nodeBaseModelType)
        {
            var nodeBaseSkin = nodeBaseModels.Find(n => n.NodeBaseModelType == nodeBaseModelType);

            if (nodeBaseSkin == null)
            {
                Debug.LogError($"Node skin not found for {nodeBaseModelType}");
                nodeBaseSkin = nodeBaseModels.Find(n => n.NodeBaseModelType == NodeBaseModelType.DefaultModel);
            }

            return nodeBaseSkin.skinName;
        }
    }
}