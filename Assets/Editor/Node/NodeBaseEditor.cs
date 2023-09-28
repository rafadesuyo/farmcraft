using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{
    [CustomEditor(typeof(NodeBase), true)]
    public class NodeBaseEditor : Editor
    {
        private const float buttonHeight = 30;

        private NodeBase nodeBase;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space();
            GUILayout.Label("Editor Actions");
            EditorGUILayout.Space();

            if (GUILayout.Button("Update model", GUILayout.Height(buttonHeight)))
            {
                nodeBase.NodeBaseModel.UpdateNodeModelInEditor(nodeBase.GetNodeBaseModelSkinType());
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Create inverse connection", GUILayout.Height(buttonHeight)))
            {
                foreach (var connection in nodeBase.Connections)
                {
                    NodeHelper.CreateInverseConnection(nodeBase, connection);
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Update name", GUILayout.Height(buttonHeight)))
            {
                nodeBase.UpdateName();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
