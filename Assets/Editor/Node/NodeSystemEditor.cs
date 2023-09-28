using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{
    [CustomEditor(typeof(NodeSystem))]
    public class NodeSystemEditor : Editor
    {
        private const float buttonHeight = 30;

        private NodeSystem nodeSystem;
        private NodeBase connectFromNode;
        private NodeBase connectToNode;
        private NodeBase spawnToNode;

        private NodeSystem.NodeType nodeType = 0;
        private NodeSystem.PawnType pawnType = 0;
        private QuizCategory quizCategory;
        private QuizDifficulty.Level quizDifficulty;
        private int questionCount = 1;

        private void OnEnable()
        {
            nodeSystem = (NodeSystem)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            DrawNodeActions();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            DrawPathActions();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            DrawPawnActions();
        }

        private void DrawNodeActions()
        {
            GUILayout.Label("Node Actions");
            GUILayout.BeginVertical("box");
            GUILayout.Label("Create new node");
            nodeType = (NodeSystem.NodeType)EditorGUILayout.EnumPopup("Node type", nodeType);
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("CREATE", GUILayout.Height(buttonHeight), GUILayout.Width(120)))
            {
                SpawnNode(GetNodeBasePrefabByType(nodeType));
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Update names", GUILayout.Height(buttonHeight)))
            {

                foreach (var nodeBase in FindObjectsByType<NodeBase>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                {
                    nodeBase.UpdateName();
                }
            }

            GUILayout.EndVertical();
        }

        public NodeBase GetNodeBasePrefabByType(NodeSystem.NodeType nodeType)
        {
            NodeBase prefab = nodeSystem.NodeElementsPrefabDatabase.GetNodeBasePrefabByType(nodeType);
            return prefab;
        }

        private void DrawPathActions()
        {
            GUILayout.Label("Path Actions");
            GUILayout.BeginVertical("box");
            GUILayout.Label("Connect nodes");
            connectFromNode = (NodeBase)EditorGUILayout.ObjectField("From", connectFromNode, typeof(NodeBase), true);
            connectToNode = (NodeBase)EditorGUILayout.ObjectField("To", connectToNode, typeof(NodeBase), true);
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("CONNECT", GUILayout.Height(buttonHeight), GUILayout.Width(120))
                && connectFromNode != null
                && connectToNode != null)
            {
                MakeConnection(connectFromNode, connectToNode);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawPawnActions()
        {
            GUILayout.Label("Pawn Actions");
            GUILayout.BeginVertical("box");
            GUILayout.Label("Add pawn to node");
            pawnType = (NodeSystem.PawnType)EditorGUILayout.EnumPopup("Pawn type", pawnType);
            questionCount = EditorGUILayout.IntField("Quiz question count", questionCount);
            quizCategory = (QuizCategory)EditorGUILayout.EnumPopup("Quiz category", quizCategory);
            quizDifficulty = (QuizDifficulty.Level)EditorGUILayout.EnumPopup("Quiz difficulty", quizDifficulty);
            spawnToNode = (NodeBase)EditorGUILayout.ObjectField("In Node", spawnToNode, typeof(NodeBase), true);
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("REMOVE", GUILayout.Height(buttonHeight), GUILayout.Width(120))
               && spawnToNode != null)
            {
                RemovePawn(spawnToNode);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("SPAWN", GUILayout.Height(buttonHeight), GUILayout.Width(120))
                && spawnToNode != null)
            {
                var pawn = SpawnPawn(pawnType, spawnToNode);

                if (pawn is QuestionPawn)
                {
                    var questionPawn = pawn as QuestionPawn;
                    questionPawn.SetQuestionOptions(questionCount,quizCategory, quizDifficulty);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void SpawnNode(NodeBase prefab)
        {
            Transform targetTransform = ((NodeSystem)target).transform;
            Transform nodeContainer = targetTransform.Find("Nodes");

            if (nodeContainer == null)
            {
                GameObject nodeContainerGameObject = new GameObject("Nodes");
                nodeContainer = nodeContainerGameObject.transform;
                nodeContainer.parent = targetTransform;
            }

            var nodeBaseObject = PrefabUtility.InstantiatePrefab(prefab, nodeContainer);
            var nodeBase = (NodeBase)nodeBaseObject;

            nodeBase.NodeBaseModel.UpdateNodeModelInEditor(nodeBase.GetNodeBaseModelSkinType());
        }

        private void MakeConnection(NodeBase connectFromNode, NodeBase connectToNode)
        {
            NodeConnection connection;

            if (NodeHelper.HasConnectionToNode(connectFromNode, connectToNode, out connection))
            {
                if (connection.Path != null)
                {
                    AssignPathToNodes(connection.Path, connectFromNode, connectToNode);
                    return;
                }
            }

            if (NodeHelper.HasConnectionToNode(connectToNode, connectFromNode, out connection))
            {
                if (connection.Path != null)
                {
                    AssignPathToNodes(connection.Path, connectToNode, connectFromNode);
                    return;
                }
            }

            SpawnPath(nodeSystem.NodeElementsPrefabDatabase.GetPathPrefabByType(NodeSystem.PathType.Default), connectFromNode, connectToNode);
        }

        private void SpawnPath(NodePath prefab, NodeBase fromNode = null, NodeBase toNode = null)
        {
            Transform targetTransform = ((NodeSystem)target).transform;
            Transform pathContainer = targetTransform.Find("Paths");

            if (pathContainer == null)
            {
                GameObject nodeContainerGameObject = new GameObject("Paths");
                pathContainer = nodeContainerGameObject.transform;
                pathContainer.parent = targetTransform;
            }

            var pathObject = PrefabUtility.InstantiatePrefab(prefab, pathContainer);

            if (fromNode == null || toNode == null)
            {
                return;
            }

            var path = pathObject as NodePath;
            AssignPathToNodes(path, fromNode, toNode);
        }

        private void AssignPathToNodes(NodePath nodePath, NodeBase fromNode, NodeBase toNode)
        {
            nodePath.transform.position = Vector3.zero;

            fromNode.SetConnection(new NodeConnection(toNode, nodePath));
            EditorUtility.SetDirty(toNode);

            toNode.SetConnection(new NodeConnection(fromNode, nodePath));
            EditorUtility.SetDirty(fromNode);

            List<Vector3> points = new List<Vector3>() {
                fromNode.transform.position,
                Vector3.Lerp(fromNode.transform.position + (Vector3)Random.insideUnitCircle, toNode.transform.position + (Vector3)Random.insideUnitCircle, 0.2f),
                Vector3.Lerp(fromNode.transform.position + (Vector3)Random.insideUnitCircle, toNode.transform.position + (Vector3)Random.insideUnitCircle, 0.8f),
                toNode.transform.position
            };

            nodePath.PathCreator.EditorData.ResetBezierPath(points.ToArray(), true);
            nodePath.CenterTransform();
            EditorUtility.SetDirty(nodePath);
        }

        private Pawn SpawnPawn(NodeSystem.PawnType pawnType, NodeBase nodeToSpawn)
        {
            RemovePawn(nodeToSpawn);

            Pawn pawnPrefab = nodeSystem.NodeElementsPrefabDatabase.GetPawnPrefabByType(pawnType);
            var pawn = PrefabUtility.InstantiatePrefab(pawnPrefab) as Pawn;
            nodeToSpawn.AddPawnToNode(pawn);
            return pawn;
        }

        private void RemovePawn(NodeBase node)
        {
            var pawns = node.GetNonPlayerPawnsInNode();

            for (int i = 0; i < pawns.Length; i++)
            {
                DestroyImmediate(pawns[i].gameObject);
            }
        }
    }
}
