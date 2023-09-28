using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Stage Info")]
public class StageInfoSO : ScriptableObject
{
    //Variables
    [Header("Information")]
    [SerializeField] private string stageName;

    [SerializeField] private int id;

    [Space(10)]
    [SerializeField] private CollectibleType collectibleNeededToPlay = CollectibleType.None;

    [SerializeField] private bool unlockWith9Collectibles = false;

    [SerializeField] private QuizCategory[] quizCategory;
    [SerializeField] private int nodeCountInStage;

    [Space(10)]
    [Header("Music")]
    [Tooltip("Background Music of the stage")]
    [SerializeField] private GameSoundSO backgroundMusic;

    [Space(10)]
    [Tooltip("The stage itself, please use the Stage Creator to change this.")]
    [SerializeField] private GameObject stage; //TODO: we can try to use the Adressables to not have this loaded in memory at all times.

    [Header("Camera Zoom")]
    [Tooltip("The minimun size of the camera in the stage, it is always less or equal the Zoom Max.")]
    [SerializeField][Range(CameraController.minZoomSize, CameraController.maxZoomSize)] private float zoomMin = 8;

    [Tooltip("The max size of the camera in the stage, it is always greater or equal the Zoom Min.")]
    [SerializeField][Range(CameraController.minZoomSize, CameraController.maxZoomSize)] private float zoomMax = 15;

    [Space(10)]
    [Tooltip("The size that the camera will start the stage with, it is always greater or equal the Zoom Min and less or equal the Zoom Max.")]
    [SerializeField] private float initialZoom = 12;

    [Header("Goals")]
    [SerializeField] private StageGoal[] goals = null;

    //Getters
    public string Name => stageName;

    public int Id => id;
    public CollectibleType CollectibleNeededToPlay => collectibleNeededToPlay;
    public bool UnlockWith9Collectibles => unlockWith9Collectibles;
    public GameObject Stage { get => stage; set => stage = value; }
    public int NodeCountInStage { get => nodeCountInStage; set => nodeCountInStage = value; }
    public QuizCategory[] CategoriesInStage { get => quizCategory; set => quizCategory = value; }

    public float ZoomMin => zoomMin;
    public float ZoomMax => zoomMax;
    public float InitialZoom => initialZoom;
    public GameSoundSO BackgroundMusic => backgroundMusic;
    public StageGoal[] Goals => goals;

    private void OnValidate()
    {
        zoomMax = Mathf.Clamp(zoomMax, CameraController.minZoomSize, CameraController.maxZoomSize);
        zoomMin = Mathf.Clamp(zoomMin, CameraController.minZoomSize, zoomMax);
        initialZoom = Mathf.Clamp(initialZoom, zoomMin, zoomMax);
    }
}