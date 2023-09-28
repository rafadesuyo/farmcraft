using DreamQuiz.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DreamQuiz
{
    public class StageManager : LocalSingleton<StageManager>
    {
        [SerializeField] private PlayerManager playerManager;

        private Dictionary<StageState.StateName, StageState> states = new Dictionary<StageState.StateName, StageState>();
        private bool isInitialized = false;
        private StageState currentStageState;

        public StageInfoSO StageInfo { get; private set; }
        public int StageId
        {
            get
            {
                return StageInfo.Id;
            }
        }
        public PlayerManager PlayerManager
        {
            get
            {
                return playerManager;
            }
        }

        public event Action OnStageInitialized;
        public event Action<StageState.StateName> OnStateEnter;
        public event Action<StageState.StateName> OnStateLeave;
        public event Action OnStageExit;

        public void SetupStage(StageInfoSO stageInfo, List<PlayerData> playerDataList)
        {
            StageInfo = stageInfo;
            playerManager?.SetupPlayers(playerDataList, StageInfo);
            StartCoroutine(InitializeStage());
        }

        public IEnumerator InitializeStage()
        {
            CameraController cameraController = FindObjectOfType<CameraController>();
            cameraController.SetCameraZoomVariables(StageInfo);

            yield return null;

            InitializeStates();
            StageSystemLocator.InitializeSystems();

            yield return null;

            OnStageInitialized?.Invoke();
            isInitialized = true;

            SetStageState(StageState.StateName.Start);
        }

        private void InitializeStates()
        {
            states.Clear();

            var allStates = Assembly.GetAssembly(typeof(StageState))
                .GetTypes()
                .Where(t => typeof(StageState).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var state in allStates)
            {
                StageState stageState = Activator.CreateInstance(state) as StageState;
                var stageName = stageState.GetStateName();

                if (states.ContainsKey(stageName))
                {
                    Debug.LogError("[StageManager] Duplicate StageState: " + stageName);
                    continue;
                }

                stageState.Initialize(this);
                states.Add(stageName, stageState);
            }
        }

        public void SetStageState(StageState.StateName stateName)
        {
            if (!isInitialized)
            {
                Debug.LogError("[StageManager] Cannot change to state:" + stateName + ". StageManager is not initialized");
                return;
            }

            if (!states.TryGetValue(stateName, out var state))
            {
                Debug.LogError("[StageManager] State not found: " + stateName);
                return;
            }

            SetStageState(state);
        }

        private void SetStageState(StageState stageState)
        {
            if (currentStageState != null)
            {
                currentStageState.OnLeave();
                OnStateLeave?.Invoke(currentStageState.GetStateName());
            }

            currentStageState = stageState;

            currentStageState.OnEnter();
            OnStateEnter?.Invoke(currentStageState.GetStateName());
        }

        public void ExitStage()
        {
            SoundManager.Instance.StopMusic();
            OnStageExit?.Invoke();
        }

        public void ReturnToWorld()
        {
            ExitStage();
            StageLoadManager.Instance.ReturnToWorldMap();
        }
    }
}