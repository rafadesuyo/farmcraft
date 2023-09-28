using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{
    [CustomEditor(typeof(QuestionDatabaseManager))]
    public class QuestionDatabaseManagerEditor : Editor
    {
        private QuestionDatabaseManager database;

        private void Awake()
        {
            database = (QuestionDatabaseManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.LabelField("Debug");
            EditorGUILayout.LabelField($"Formatter is ready: {database.IsFormatterReady}");
            EditorGUILayout.LabelField($"Repository is ready: {database.IsRepositoryReady}");
        }
    }
}