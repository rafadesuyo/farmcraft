using DreamQuiz;
using UnityEngine;

public class StageHolder : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private BoxCollider2D cameraBounds;

    //Getters
    public BoxCollider2D CameraBounds => cameraBounds;

    public void UpdateSplinesPathMesh()
    {
        SplinePath[] splines = GetComponentsInChildren<SplinePath>();

        foreach (SplinePath spline in splines)
        {
            spline.PathMeshCreator.TriggerUpdate();
        }
    }
}
