using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DreamQuiz
{
    [CustomEditor(typeof(NodePathModel))]
    public class NodePathModelEditor : Editor
    {
        private const float buttonHeight = 30;
        private NodePathModel nodePathModel;

        private void Awake()
        {
            nodePathModel = (NodePathModel)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Delete path tiles", GUILayout.Height(buttonHeight)))
            {
                ClearTiles();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Create path tiles", GUILayout.Height(buttonHeight)))
            {
                ClearTiles();
                CreateTiles();
            }
        }

        public void CreateTiles()
        {
            GameObject prefab = null;

            for (int i = 0; i < nodePathModel.TileCount; i++)
            {
                prefab = nodePathModel.GetRandomTile(prefab);
                GameObject tile = PrefabUtility.InstantiatePrefab(prefab, nodePathModel.ModelContainer) as GameObject;

                float t = 0;

                if (i > 0)
                {
                    t = (float)i / (nodePathModel.TileCount - 1);
                }

                Vector3 position = nodePathModel.GetPointAtTime(t) + (Vector3)(Random.insideUnitCircle * 0.1f);
                tile.transform.position = position;

                Vector3 aheadPosition = nodePathModel.GetPointAtTime(Mathf.Clamp(t + 0.1f, 0, 1));
                Vector3 direction = NodeHelper.GetDirectionFromVector(aheadPosition - position);
                float angleRad = Mathf.Atan2(direction.y, direction.x);
                float angleDeg = angleRad * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angleDeg - 90);
                tile.transform.rotation = targetRotation;
            }
        }

        private void ClearTiles()
        {
            List<GameObject> childList = new List<GameObject>();

            foreach (Transform child in nodePathModel.ModelContainer)
            {
                childList.Add(child.gameObject);
            }

            for (int i = 0; i < childList.Count; i++)
            {
                DestroyImmediate(childList[i]);
            }
        }
    }
}