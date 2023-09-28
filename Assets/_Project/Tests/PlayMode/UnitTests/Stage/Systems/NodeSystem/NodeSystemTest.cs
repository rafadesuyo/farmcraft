using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class NodeSystemTest : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator Initialize_HappyPath()
        {
            //Arrange
            var nodeSystemGameObject = new GameObject("NodeSystem");
            var nodeSystem = nodeSystemGameObject.AddComponent<NodeSystem>();

            //Act
            yield return null;
            bool isSystemRegistered = StageSystemLocator.IsSystemRegistered<NodeSystem>();

            //Assert
            Assert.IsTrue(nodeSystem.IsReady,
                $"[Initialize] System not initialized");

            Assert.IsTrue(isSystemRegistered,
                $"[Initialize] System not registered");

            Destroy(nodeSystem.gameObject);
        }

        [UnityTest]
        public IEnumerator Initialize_MoreThanOneSystemInScene()
        {
            //Arrange
            var nodeSystemGameObject = new GameObject("NodeSystem");
            var nodeSystem = nodeSystemGameObject.AddComponent<NodeSystem>();

            yield return null;
            List<NodeSystem> nodeSystemList = new List<NodeSystem>();

            for (int i = 0; i < 2; i++)
            {
                var gameObject = new GameObject($"NodeSystem {i + 1}");
                var duplicateNodeSystem = gameObject.AddComponent<NodeSystem>();
                nodeSystemList.Add(duplicateNodeSystem);
            }

            //Act
            yield return null;
            var fetchedNodeSystem = StageSystemLocator.GetSystem<NodeSystem>();

            //Assert
            for (int i = 0; i < nodeSystemList.Count; i++)
            {
                Assert.IsFalse(
                      nodeSystemList[i].IsReady,
                    $"[MoreThanOneSystemInScene] Additional system number {i} was initialized");
            }

            for (int i = 0; i < nodeSystemList.Count; i++)
            {
                Assert.AreNotEqual(
                    fetchedNodeSystem,
                    nodeSystemList[i],
                    $"[MoreThanOneSystemInScene] Additional system number {i} was registered");
            }

            Destroy(nodeSystem.gameObject);
        }

        [UnityTest]
        public IEnumerator RegisterNode_HappyPath()
        {
            var nodeBase = Resources.Load("Prefabs/Nodes/NodeBase/NodeBase", typeof(NodeBase)) as NodeBase;
            Instantiate(nodeBase, Vector3.zero, Quaternion.identity);

            yield return null;

            var nodeSystemGameObject = new GameObject("NodeSystem");
            var nodeSystem = nodeSystemGameObject.AddComponent<NodeSystem>();

            yield return null;

            Assert.IsFalse(nodeSystem.StageNodes.Count == 0,
                $"[RegisterNode] No NodeBases were registered to the NodeSystem");

            Assert.IsFalse(nodeSystem.StageNodes.Count > 1,
                $"[RegisterNode] More than 1 NodeBase were registered to the NodeSystem");

            Destroy(nodeSystem.gameObject);
        }
    }
}