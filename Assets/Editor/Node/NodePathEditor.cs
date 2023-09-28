using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{
    [CustomEditor(typeof(NodePath))]
    public class NodePathEditor : Editor
    {
        private const float buttonHeight = 30;

        private NodePath nodePath;

        private void Awake()
        {
            nodePath=(NodePath)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Center transform", GUILayout.Height(buttonHeight)))
            {
                nodePath.CenterTransform();
            }
        }
    }
}