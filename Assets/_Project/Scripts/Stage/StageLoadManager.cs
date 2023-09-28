using System;
using UnityEngine;

namespace DreamQuiz
{
    public class StageLoadManager : LocalSingleton<StageLoadManager>
    {
        //Variables
        [Header("Variables")]
        [SerializeField] private string initalScene = "01 - InitialScene";
        [SerializeField] private string homeMenuScene = "02 - HomeMenu";
        private IStageFinalizer stageFinalizer;

        public void LoadStage(IStageInitializer stageInitializer = null, IStageFinalizer stageFinalizer = null)
        {
            this.stageFinalizer = stageFinalizer;
            LoadScene(stageInitializer.GetStageScene(), stageInitializer.InitializeStage);
        }

        public void ReturnToInitialScene()
        {
            LoadScene(initalScene, GameManager.Instance.LoadGame);
        }

        public void ReturnToHomeMenu()
        {
            ReturnToHomeMenu(null);
        }

        public void ReturnToWorldMap()
        {
            stageFinalizer?.FinalizeStage();
            ReturnToHomeMenu(() => CanvasManager.Instance.OpenMenu(Menu.WorldMap, null, true));
        }

        public void ReturnToHomeMenu(Action onFinishStageLoad)
        {
            LoadScene(homeMenuScene, () =>
            {
                CanvasManager.Instance.OpenMenu(Menu.Home);
                onFinishStageLoad?.Invoke();
            });
        }

        private void LoadScene(string sceneName, Action onFinishStageLoad = null)
        {
            SceneLoader.Instance.LoadScene(sceneName, onFinishStageLoad);
        }
    }
}