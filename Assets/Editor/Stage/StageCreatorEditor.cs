using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageCreator), true)]
public class StageCreatorEditor : Editor
{
    private readonly Vector2 goalsWindownPosition = new Vector2(20, 20);
    private const float goalsWindownHorizontalSize = 200f;

    private StageCreator stageCreatorEditor;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(20);

        stageCreatorEditor = (StageCreator)target;

        if (GUILayout.Button("Save Stage"))
        {
            stageCreatorEditor.SaveStage();
        }

        if (GUILayout.Button("Load Stage"))
        {
            stageCreatorEditor.LoadStage();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Reset Stage"))
        {
            stageCreatorEditor.ResetStage();
        }
    }

    public void OnSceneGUI()
    {
        Handles.BeginGUI();

        if (stageCreatorEditor == null)
        {
            return;
        }

        var stageGoals = stageCreatorEditor.StageInfo.Goals;

        //The vertical size is EditorGUIUtility.singleLineHeight * how much line there will be:
        //Title -> spacing line -> one line for each goal -> spacing line, which is:
        //stageGoals.Length + 3 (title + space + stageGoals.Length + space) * singleLineHeight
        float goalsWindownVerticalSize = (stageGoals.Length + 3) * EditorGUIUtility.singleLineHeight;
        GUILayout.BeginArea(new Rect(goalsWindownPosition.x, goalsWindownPosition.y, goalsWindownHorizontalSize, goalsWindownVerticalSize));

        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.yellow;
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        DrawTextInFlexibleSpace("Stage Goals");
        GUILayout.EndHorizontal();

        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        foreach (StageGoal goal in stageGoals)
        {
            GUILayout.BeginHorizontal();
            DrawTextInFlexibleSpace($"{goal.Requisite} = {goal.TargetValue}");
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    private void DrawTextInFlexibleSpace(string text)
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label($"{text}");
        GUILayout.FlexibleSpace();
    }
}
