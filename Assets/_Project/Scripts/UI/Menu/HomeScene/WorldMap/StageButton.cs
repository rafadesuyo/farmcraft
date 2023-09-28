using UnityEngine;

[SelectionBase]
public class StageButton : MonoBehaviour, IClickablePointerDown
{
    //Components
    [Header("Components")]
    [SerializeField] private Animator animator;

    //Enums
    public enum StageState { Inactive, Incompleted, Completed }

    //Events
    public delegate void StageButtonEvent(StageButton stageButton);

    public StageButtonEvent OnStageButtonSelected;

    //Variables
    [Header("Variables")]
    [SerializeField] private StageInfoSO stageInfo;

    [Space(10)]

    [SerializeField] private bool unlockInSpecificNumberOfStagesCompleted = false;
    [SerializeField] private int numberOfStagesToUnlock;

    private StageState state;

    //Getters
    public StageInfoSO StageInfo => stageInfo;
    public bool UnlockInSpecificNumberOfStagesCompleted => unlockInSpecificNumberOfStagesCompleted;
    public int NumberOfStagesToUnlock => numberOfStagesToUnlock;
    public StageState State => state;

    private void OnEnable()
    {
        UpdateAnimation();
    }

    public void Init(StageState newState)
    {
        state = newState;

        UpdateVariables();
    }

    private void UpdateVariables()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (transform.gameObject.activeInHierarchy == false)
        {
            return;
        }

        animator.SetFloat("State", (float)state);
    }

    public void SelectStage()
    {
        OnStageButtonSelected?.Invoke(this);

        if (state == StageState.Inactive)
            AudioManager.Instance.Play("LockedNode");
        else
            AudioManager.Instance.Play("UnlockedNode");
    }

    public void OnPointerDown()
    {
        SelectStage();
    }
}
