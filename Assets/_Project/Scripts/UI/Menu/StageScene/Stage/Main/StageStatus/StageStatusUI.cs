using System.Collections.Generic;
using UnityEngine;

public class StageStatusUI : MonoBehaviour
{
    [SerializeField] private GameObject stageStatusItemPrefab = null;
    [SerializeField] private RectTransform stageStatusContainer = null;
    [SerializeField] private StageStatusDatabase statusDatabase = null;
    [SerializeField] private RectTransform stageStatusBackground = null;

    private Dictionary<StageStatus, StageStatusItem> currentStageStatus = new Dictionary<StageStatus, StageStatusItem>();

    private void Awake()
    {
        stageStatusBackground.gameObject.SetActive(false);

        GenericPool.CreatePool<StageStatusItem>(stageStatusItemPrefab, stageStatusContainer);
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onUpdateStageStatus, OnUpdateStageStatus);
        EventsManager.AddListener(EventsManager.onSelectStageStatusIcon, OnSelectStageStatusIcon);
        EventsManager.AddListener(EventsManager.onDeselectStageStatusIcon, OnDeselectStageStatusIcon);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onUpdateStageStatus, OnUpdateStageStatus);
        EventsManager.RemoveListener(EventsManager.onSelectStageStatusIcon, OnSelectStageStatusIcon);
        EventsManager.RemoveListener(EventsManager.onDeselectStageStatusIcon, OnDeselectStageStatusIcon);
    }

    private void OnUpdateStageStatus(IGameEvent gameEvent)
    {
        OnUpdateStageStatusEvent stageStatusEvent = ((OnUpdateStageStatusEvent)gameEvent);

        if (stageStatusEvent.type == StageStatus.None)
        {
            return;
        }

        StageStatusItem statusItem = null;

        if (!currentStageStatus.ContainsKey(stageStatusEvent.type))
        {
            currentStageStatus.Add(stageStatusEvent.type, GenericPool.GetItem<StageStatusItem>());
        }

        statusItem = currentStageStatus[stageStatusEvent.type];

        if (string.IsNullOrEmpty(stageStatusEvent.value))
        {
            statusItem.ReleaseItem();
            currentStageStatus.Remove(stageStatusEvent.type);
            return;
        }

        StageStatusDataPair stageStatusDataPair = statusDatabase.GetStageStatusPairOfType(stageStatusEvent.type);
        statusItem.UpdateItem(stageStatusDataPair.Icon, stageStatusEvent.value, stageStatusDataPair.Title, stageStatusDataPair.Description);
    }

    private void OnSelectStageStatusIcon(IGameEvent gameEvent)
    {
        stageStatusBackground.gameObject.SetActive(true);
    }

    private void OnDeselectStageStatusIcon(IGameEvent gameEvent)
    {
        stageStatusBackground.gameObject.SetActive(false);
    }
}