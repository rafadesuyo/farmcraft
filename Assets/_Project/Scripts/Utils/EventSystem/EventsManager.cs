
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEngine;
#endif

public class EventsManager
{
#if UNITY_EDITOR
    private static readonly bool SHOW_DEBUG = false;
#endif

    #region Listerners 

    // Scene Change
    public const string onSceneLoad = "onSceneLoad";

    // Server
    public const string onStartRequest = "onStartRequest";
    public const string onEndAllRequests = "onEndAllRequests";

    // Leaderboard
    public const string onFetchLeaderboard = "onFetchLeaderboard";

    // Collectible
    public const string onUnlockCollectible = "onUnlockCollectible";
    public const string onLevelUpCollectible = "onLevelUpCollectible";

    // Game Resourcers
    public const string onGoldChange = "onGoldChange";
    public const string onHeartChange = "onHeartChange";
    public const string onOneekoinChange = "onOneekoinChange";
    public const string onLullabyNoteChange = "onLullabyNoteChange";

    // Collectibles
    public const string onUpdateTeam = "onUpdateTeam";
    public const string onSelectNewTeamMember = "onSelectNewTeamMember";

    // Stage-Related
    public const string onDreamEnergyChange = "onDreamEnergyChange";
    public const string onSleepingTimeChange = "onSleepingTimeChange";
    public const string onSelectCollectibleAbility = "onSelectCollectibleAbility";
    public const string onDeselectCollectibleAbility = "onDeselectCollectibleAbility";
    public const string onUpdateStageStatus = "onUpdateStageStatus";
    public const string onSelectStageStatusIcon = "onSelectStageStatusIcon";
    public const string onDeselectStageStatusIcon = "onDeselectStageStatusIcon";
    public const string onCutAnswer = "onCutAnswer";
    public const string onFindSecretPath = "onFindSecretPath";
    public const string onBeatStage = "onBeatStage";
    public const string onCompleteStage = "onCompleteStage";
    public const string onBeatAllFarmingStages = "onBeatAllFarmingStages";
    public const string onWalkPath = "onWalkPath";
    public const string onUpdateStageGoal = "onUpdateStageGoal";
    // The act of selecting an answer.
    public const string onPlayerSelectAnswer = "onPlayerChooseAnswer";
    // When an answer is processed and the question is answered.
    public const string onQuestionIsAnswered = "onQuestionIsAnswered";
    public const string onQuestionIsCorrectlyAnswered = "onQuestionIsCorrectlyAnswered";
    public const string onQuestionIsWronglyAnswered = "onQuestionIsWronglyAnswered";
    public const string onSelectNode = "onSelectNode";
    public const string onDeselectNode = "onDeselectNode";

    // TODO: If we need more events related to view open, create a generic one and pass the view.
    public const string onOpenQuizView = "onOpenQuizView";
    public const string onOpenStageView = "onOpenStageView";
    public const string onOpenWorldMapView = "onOpenWorldMapView";

    // Abilities
    public const string onStartListeningForTarget = "onStartListeningForTarget";
    public const string onStoptListeningForTarget = "onStoptListeningForTarget";
    public const string onFindTarget = "onFindTarget";

    // TODO: Review. Maybe change for only one event and the receiver checks for the target?
    // This can add a bunch of checks in a lot of places, can be bad.
    public const string onUseSleepingTimeAbility = "onUseSleepingTimeAbility";
    public const string onUseQuizTimeAbility = "onUseQuizTimeAbility";
    public const string onUseQuestionChangeAbility = "onUseQuestionChangeAbility";
    public const string onUseDifficultyChangeAbility = "onUseDifficultyChangeAbility";

    // Missions
    public const string onMissionCanBeCompleted = "onMissionCanBeCompleted";
    public const string onMissionIsCompleted = "onMissionIsCompleted";

    #endregion

    private static Dictionary<string, List<Action<IGameEvent>>> _eventDictionary = new Dictionary<string, List<Action<IGameEvent>>>();

    public static void AddListener(string eventName, Action<IGameEvent> callbackToAdd)
    {
        if (!_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
        {
            callbackList = new List<Action<IGameEvent>>();
            _eventDictionary.Add(eventName, callbackList);
#if UNITY_EDITOR
            if (SHOW_DEBUG)
            {
                Debug.LogWarning($"There is no event called: {eventName}. Creating a new one.");
            }
#endif
        }
#if UNITY_EDITOR
        else if (SHOW_DEBUG)
        {
            Debug.Log($"Event: {eventName}. Adding {callbackToAdd.Method} from {callbackToAdd.Target}.");
        }
#endif

        callbackList.Add(callbackToAdd);
    }

    public static void RemoveListener(string eventName, Action<IGameEvent> callbackToRemove)
    {
        if (_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
        {
            callbackList?.Remove(callbackToRemove);

#if UNITY_EDITOR
            if (SHOW_DEBUG)
            {
                Debug.Log($"Event: {eventName}. Removing {callbackToRemove.Method} from {callbackToRemove.Target}.");
            }
#endif
        }
#if UNITY_EDITOR
        else if (SHOW_DEBUG)
        {
            Debug.LogWarning($"There is no event called: {eventName}.");
        }
#endif
    }


    public static void Publish(string eventName, IGameEvent eventInfos = null)
    {
        if (_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
        {
            Action<IGameEvent>[] callbackListAux = callbackList.ToArray();

            foreach (var callback in callbackListAux)
            {
                callback?.Invoke(eventInfos);
                if (callback != null)
                {
#if UNITY_EDITOR
                    if (SHOW_DEBUG)
                    {
                        Debug.Log($"Event: {eventName}. Calling {callback.Method} from {callback.Target}.");
                    }
#endif
                }
#if UNITY_EDITOR
                else if (SHOW_DEBUG)
                {
                    Debug.LogWarning($"Event: {eventName}. Some callback is null.");
                }
#endif
            }
        }
    }
}