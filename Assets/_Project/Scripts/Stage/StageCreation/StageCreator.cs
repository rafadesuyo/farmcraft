using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamQuiz;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageCreator : MonoBehaviour
{
    //Constants
    private string scriptableObjectType = ".asset";
    private string prefabType = ".prefab";
    private string prefabIdentifier = "_StagePrefab";

    private string stageObjectName = "Stage";

    //Components
    [Header("Components")]
    [SerializeField] private GameObject stageBase;

    //Variables
    [Header("Variables")]

    [Tooltip("The stage to edit.")]
    [SerializeField] private StageInfoSO stageInfo;

#if UNITY_EDITOR
    //This is only to use for the custom inspector for now
    public StageInfoSO StageInfo => stageInfo;
#endif

    private StageHolder stage;

#if UNITY_EDITOR
    //Getters
    private StageHolder CurrentStage
    {
        get
        {
            if (stage != null)
            {
                return stage;
            }
            else
            {
                FindCurrentStage();

                if (stage != null)
                {
                    return stage;
                }
                else
                {
                    CreateNewStage();
                    return stage;
                }
            }
        }
    }

    private void FindCurrentStage()
    {
        if (stage == null)
        {
            stage = FindObjectOfType<StageHolder>();
        }
    }

    private void CreateNewStage(GameObject stagePrefab = null)
    {
        GameObject stageGameObject;

        if (stagePrefab == null)
        {
            stageGameObject = PrefabUtility.InstantiatePrefab(stageBase) as GameObject;
        }
        else
        {
            stageGameObject = PrefabUtility.InstantiatePrefab(stagePrefab) as GameObject;
        }

        UnpackPrefab(stageGameObject);

        stage = stageGameObject.GetComponent<StageHolder>();

        stage.name = stageObjectName;

        stage.transform.position = Vector3.zero;

        stage.transform.SetAsLastSibling();

        stage.UpdateSplinesPathMesh();
    }

    public void SaveStage()
    {
        if (stageInfo == null)
        {
            Debug.LogWarning("The Stage Info is empty, please choose one to save the stage.");
            return;
        }

        EditorUtility.SetDirty(stageInfo);

        StageHolder _stage = CurrentStage;

        UnpackPrefab(_stage.gameObject);

        string scriptableName = stageInfo.name + scriptableObjectType;
        string prefabName = stageInfo.name + prefabIdentifier + prefabType;

        var nodeBases = FindObjectsOfType<NodeBase>();
        stageInfo.NodeCountInStage = nodeBases.Length;

        List<QuizCategory> categories = new List<QuizCategory>();

        foreach (var nodeBase in nodeBases)
        {
            var pawnwInNode = nodeBase.GetNonPlayerPawnsInNode();

            foreach (var pawn in pawnwInNode)
            {
                var questionPawn = pawn as QuestionPawn;

                if (questionPawn != null)
                {
                    if (!categories.Contains(questionPawn.QuizCategory))
                    {
                        categories.Add(questionPawn.QuizCategory);
                    }
                }
            }
        }

        stageInfo.CategoriesInStage = categories.ToArray();

        string scriptablePath = AssetDatabase.GetAssetPath(stageInfo);

        string folderPath = scriptablePath.Remove(scriptablePath.Length - scriptableName.Length, scriptableName.Length);

        string prefabPath = folderPath + prefabName;

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(_stage.gameObject, prefabPath, out bool prefabSuccess);

        if (prefabSuccess == true)
        {
            Debug.Log($"The stage prefab was saved to \"{stageInfo.name}\" successfully!");
        }
        else
        {
            Debug.Log($"Failed to save the stage prefab to \"{stageInfo.name}\" .");
            return;
        }

        stageInfo.Stage = prefab;

        UnityEditor.AssetDatabase.SaveAssets();
    }

    public void LoadStage()
    {
        if (stageInfo == null)
        {
            Debug.LogWarning("The Stage Info is empty, please choose one to load the stage.");
            return;
        }

        if (stageInfo.Stage == null)
        {
            Debug.LogWarning("The Stage Info doesn't have a Stage prefab to load.");
            return;
        }

        InstantiateStage(stageInfo.Stage);

        Debug.Log($"The stage prefab from \"{stageInfo.name}\" was loaded successfully.");
    }

    public void ResetStage()
    {
        InstantiateStage();
    }

    private void DeleteCurrentStage()
    {
        FindCurrentStage();

        if (stage != null)
        {
            DestroyImmediate(stage.gameObject);

            stage = null;
        }
    }

    private void InstantiateStage(GameObject stagePrefab = null)
    {
        DeleteCurrentStage();
        CreateNewStage(stagePrefab);
    }

    private void UnpackPrefab(GameObject stageGameObject)
    {
        if (PrefabUtility.IsPartOfAnyPrefab(stageGameObject) == true)
        {
            PrefabUtility.UnpackPrefabInstance(stageGameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }
    }
#endif
}
