using UnityEngine;

namespace DreamQuiz
{
    public class StoryModeStageInitializer : IStageInitializer
    {
        private const string stageScene = "03 - StageScene";

        private StoryModeInitializerData storyModeInitializerData;

        public void InitializeStage()
        {
            HeartManager.Instance.UseHeart();

            StageHolder stage = GameObject.Instantiate(storyModeInitializerData.StageInfoSO.Stage).GetComponent<StageHolder>();

            stage.transform.position = Vector3.zero;
            stage.transform.SetAsLastSibling();

            CanvasManager.Instance.OpenMenu(Menu.Stage);
            StageManager.Instance.SetupStage(storyModeInitializerData.StageInfoSO, storyModeInitializerData.PlayerDataList);
        }

        public string GetStageScene()
        {
            return stageScene;
        }

        public void SetupInitializer(object data)
        {
            storyModeInitializerData = data as StoryModeInitializerData;

            if (storyModeInitializerData == null)
            {
                Debug.LogError($"[StoryModeStageInitializer] StoryModeInitializerData not valid");
            }
        }
    }
}