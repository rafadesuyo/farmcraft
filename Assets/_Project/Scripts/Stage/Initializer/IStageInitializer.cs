namespace DreamQuiz
{
    public interface IStageInitializer
    {
        void SetupInitializer(object data);
        string GetStageScene();
        void InitializeStage();
    }
}