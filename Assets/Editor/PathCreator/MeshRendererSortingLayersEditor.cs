using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;

//Expose SortingLayer  SortingOrder on MeshRenderer
//With nice drop down and revert to prefab functionality.

//Base exposing code by neror http://forum.unity3d.com/threads/212006-Drawing-order-of-Meshes-and-Sprites
//Get all sorting layer name and ID by guavaman  Ivan.Murashko http://answers.unity3d.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html
//Sorting Layer drop down menu, bold text on prefab override, revert to prefab and instant update on Order change functionality by 5argon

//Modifications made by Kelvin using scripts from: https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/

[CustomEditor(typeof(MeshRenderer))]
[CanEditMultipleObjects]

public class MeshRendererSortingLayersEditor : Editor
{
    Editor defaultEditor;
    MeshRenderer _targetMeshRenderer;

    private void OnEnable()
    {
        Type editorType = Type.GetType("UnityEditor.MeshRendererEditor, UnityEditor");

        if (editorType == null)
        {
            throw new Exception("O tipo de editor \"UnityEditor.MeshRendererEditor\" nao foi encontrado!");
        }

        //When this inspector is created, also create the built-in inspector
        defaultEditor = CreateEditor(targets, editorType);
        _targetMeshRenderer = target as MeshRenderer;
    }

    private void OnDisable()
    {
        //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
        //Also, make sure to call any required methods like OnDisable

        MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (disableMethod != null)
            disableMethod.Invoke(defaultEditor, null);

        DestroyImmediate(defaultEditor);
    }

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        defaultEditor.OnInspectorGUI();

        serializedObject.Update();

        SerializedProperty sortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
        SerializedProperty sortingOrder = serializedObject.FindProperty("m_SortingOrder");

        MeshRenderer renderer = target as MeshRenderer;

        EditorGUILayout.Space(10);

        Rect firstHoriz = EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginProperty(firstHoriz, GUIContent.none, sortingLayerID);

        string[] layerNames = GetSortingLayerNames();
        int[] layerID = GetSortingLayerUniqueIDs();

        int selected = -1;
        //What is selected?
        int sID = sortingLayerID.intValue;
        for (int i = 0; i < layerID.Length; i++)
        {
            //Debug.Log(sID + " " + layerID[i]);
            if (sID == layerID[i])
            {
                selected = i;
            }
        }

        if (selected == -1)
        {
            //Select Default.
            for (int i = 0; i < layerID.Length; i++)
            {
                if (layerID[i] == 0)
                {
                    selected = i;
                }
            }
        }

        selected = EditorGUILayout.Popup("Sorting Layer", selected, layerNames);

        //Translate to ID
        sortingLayerID.intValue = layerID[selected];


        EditorGUI.EndProperty();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(sortingOrder, new GUIContent("Order in Layer"));


        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
}