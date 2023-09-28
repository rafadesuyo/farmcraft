namespace DreamQuiz
{
    public class TrialModeStageInitializer : IStageInitializer
    {
        private const string stageScene = "04 - TrialMode";

        public void InitializeStage() { }

        public string GetStageScene()
        {
            return stageScene;
        }

        public void SetupInitializer(object data) { }
    }
}