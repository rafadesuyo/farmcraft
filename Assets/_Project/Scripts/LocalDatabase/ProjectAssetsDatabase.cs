using UnityEngine;

public class ProjectAssetsDatabase : LocalSingleton<ProjectAssetsDatabase>
{
    [SerializeField] private StageGoalDataSO stageGoalDatabase;
    [SerializeField] private CategoryDatabaseSO categoryDatabase;
    [SerializeField] private StageRewardDataSO stageRewardDatabase;
    [SerializeField] private CollectibleShardDataSO collectibleShardDatabase;

    public StageGoalDataSO.StageGoalDataPair GetStageGoalDataByRequisite(StageGoal.StageGoalRequisite requisite)
    {
        return Instance.stageGoalDatabase.GetDataByRequisite(requisite);
    }

    public Color GetCategoryColor(QuizCategory category)
    {
        return Instance.categoryDatabase.GetColorByCategory(category);
    }

    public Sprite GetCategoryIcon(QuizCategory category)
    {
        return Instance.categoryDatabase.GetIconByCategory(category);
    }

    public string GetCategoryName(QuizCategory category)
    {
        return Instance.categoryDatabase.GetNameByCategory(category);
    }

    public Sprite GetCollectibleShardIcon(CollectibleType collectibleType)
    {
        return collectibleShardDatabase.GetCollectibleShardIconByType(collectibleType);
    }

    public Sprite GetStageRewardIcon(CurrencyType type, CollectibleType collectible = CollectibleType.None)
    {
        return stageRewardDatabase.GetIconByTypeAndCollectible(type, collectible);
    }
}
