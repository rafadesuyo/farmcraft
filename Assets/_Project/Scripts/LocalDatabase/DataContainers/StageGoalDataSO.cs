using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stage Goal Data")]
public class StageGoalDataSO : ScriptableObject
{
    //Variables
    [SerializeField] private StageGoalDataPair[] stageGoalDataPairs;

    public StageGoalDataPair GetDataByRequisite(StageGoal.StageGoalRequisite requisite)
    {
        foreach (StageGoalDataPair stageGoalDataPair in stageGoalDataPairs)
        {
            if (stageGoalDataPair.Requisite == requisite)
            {
                return stageGoalDataPair;
            }
        }

        throw new System.Exception($"The requisite \"{requisite}\" isn't present in the data list!");
    }

    [System.Serializable]
    public class StageGoalDataPair
    {
        //Variables
        [SerializeField] private StageGoal.StageGoalRequisite requisite;

        [Space(10)]

        [SerializeField] private Sprite goalIcon;
        [SerializeField] private Sprite screenGoalIcon;

        [SerializeField] private string goalTextStageInfoView;

        //Getters
        public StageGoal.StageGoalRequisite Requisite => requisite;
        public Sprite GoalIcon => goalIcon;
        public Sprite ScreenGoalIcon => screenGoalIcon;
        public string GoalTextStageInfoView => goalTextStageInfoView;
    }
}
