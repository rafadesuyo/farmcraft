using System;
using System.Collections.Generic;
using UnityEngine;


public enum StageStatus
{
    None,
    Key,
    Power,
    Wolf,
    Sheep,
    Poison,
    Defense,
    ReducePoison,
    ReduceWalkSleepingTime,
    ReduceWrongAnswerSleepingTime
}

public class StatusUI : MonoBehaviour
{
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private StageStatusDatabase statusDatabase = null;

    private Dictionary<StageStatus, StatusSlot> currentStatus = new Dictionary<StageStatus, StatusSlot>();

    private float forwardAngle = -40f;
    private float backwardsAngle = 40f;

    public static event Action OnWheelRotate;

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onUpdateStageStatus, CreateSlot);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onUpdateStageStatus, CreateSlot);
    }

    private void Awake()
    {
        GenericPool.CreatePool<StatusSlot>(SlotPrefab, gameObject.transform);
    }

    private void CreateSlot(IGameEvent gameEvent)
    {
        OnUpdateStageStatusEvent stageStatusEvent = ((OnUpdateStageStatusEvent)gameEvent);

        if (stageStatusEvent.type == StageStatus.None)
        {
            return;
        }

        StatusSlot newSlot = null;

        if (!currentStatus.ContainsKey(stageStatusEvent.type))
        {
            newSlot = GenericPool.GetItem<StatusSlot>();
            currentStatus.Add(stageStatusEvent.type, newSlot);

            newSlot.transform.SetParent(transform, false);
            newSlot.transform.SetAsFirstSibling();
            RotateWheelForward();
        }

        newSlot = currentStatus[stageStatusEvent.type];

        if (string.IsNullOrEmpty(stageStatusEvent.value))
        {
            newSlot.ReleaseItem();
            currentStatus.Remove(stageStatusEvent.type);
            RotateWheelBackwards(newSlot.gameObject.transform);
            return;
        }

        StageStatusDataPair stageStatusDataPair = statusDatabase.GetStageStatusPairOfType(stageStatusEvent.type);
        newSlot.UpdateSlotStatus(stageStatusDataPair.Icon);
    }

    private void RotateWheelForward()
    {
        foreach (Transform slot in transform)
        {
            slot.Rotate(0, 0, forwardAngle);
        }

        OnWheelRotate?.Invoke();
    }

    private void RotateWheelBackwards(Transform removedSlot)
    {
        Transform parentTransform = removedSlot.parent;

        foreach (Transform slot in parentTransform)
        {
            if (slot.GetSiblingIndex() > removedSlot.GetSiblingIndex())
            {
                slot.Rotate(0, 0, backwardsAngle);
            }
        }

        OnWheelRotate?.Invoke();
    }
}