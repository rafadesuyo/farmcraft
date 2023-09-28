using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class PlayerStageInstance : MonoBehaviour
    {
        public int stageId { get; private set; }
        public int Id { get; private set; }
        public PlayerPawn PlayerPawn { get; private set; }
        public PlayerStageData PlayerStageData { get; private set; }
        public PlayerStageGoal PlayerStageGoal { get; private set; }
        public PlayerStageAbility PlayerStageAbility { get; private set; }
        public PlayerStageQuizHandler PlayerStageQuizHandler { get; private set; }

        private PlayerManager playerManager;
        private CameraController cameraController;

        public event Action OnInitialized;
        public event Action OnActivate;
        public event Action OnDeactivate;

        public void InitializePlayer(PlayerManager playerManager, PlayerData playerData, StageInfoSO stageInfo)
        {
            this.playerManager = playerManager;

            Id = playerData.Id;
            stageId = stageInfo.Id;
            PlayerStageData = new PlayerStageData(playerData);
            PlayerStageGoal = new PlayerStageGoal(PlayerStageData, stageInfo.Goals);
            PlayerStageAbility = new PlayerStageAbility(PlayerStageData, playerData.Team);
            PlayerStageQuizHandler = new PlayerStageQuizHandler(this, StageSystemLocator.GetSystem<QuizSystem>());

            PlayerPawn = Instantiate(playerData.PlayerPawnPrefab, transform);
            PlayerPawn.Initialize(this);

            PlayerStageGoal.OnGoalStateChange += PlayerStageGoal_OnGoalStateChange;

            OnInitialized?.Invoke();
        }

        private void PlayerStageGoal_OnGoalStateChange(PlayerStageGoal.PlayerStageGoalState state)
        {
            if (state == PlayerStageGoal.PlayerStageGoalState.Unfinished)
            {
                return;
            }

            StageManager.Instance.SetStageState(StageState.StateName.End);
            Deactivate();

            switch (state)
            {
                case PlayerStageGoal.PlayerStageGoalState.Lose:
                    AudioManager.Instance.Play("LoseSound");
                    break;
                case PlayerStageGoal.PlayerStageGoalState.Win:
                    AudioManager.Instance.Play("VictorySound");
                    break;
            }

            OpenResultsScreen();
        }

        private void OpenResultsScreen()
        {
            CanvasManager.Instance.OpenMenu(Menu.StageResult);
        }

        public void Activate()
        {
            playerManager.SetCurrentPlayerInstance(this);

            if (cameraController == null)
            {
                cameraController = FindObjectOfType<CameraController>();
            }

            cameraController.SetCameraFollow(PlayerPawn.transform);

            OnActivate?.Invoke();
        }

        public void Deactivate()
        {
            OnDeactivate?.Invoke();
        }
    }
}