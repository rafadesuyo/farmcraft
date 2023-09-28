using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class NodeBaseTest : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator RegisterNode_HappyPath()
        {
            var nodeBase = Resources.Load("Prefabs/Nodes/NodeBase/NodeBase", typeof(NodeBase)) as NodeBase;
            var nodeBaseInstance = Instantiate(nodeBase, Vector3.zero, Quaternion.identity);

            yield return null;

            var nodeSystemGameObject = new GameObject("NodeSystem");
            nodeSystemGameObject.AddComponent<NodeSystem>();

            yield return null;

            Assert.IsTrue(nodeBaseInstance.IsInitialized,
                $"[NodeBase] The NodeBase wasn't initialized");

            Destroy(nodeSystemGameObject); 
        }
    }
}