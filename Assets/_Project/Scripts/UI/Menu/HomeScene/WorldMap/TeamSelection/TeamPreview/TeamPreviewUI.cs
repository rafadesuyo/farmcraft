using System.Collections.Generic;
using UnityEngine;
using System;

public class TeamPreviewUI : MonoBehaviour
{
    [SerializeField] private List<TeamPreviewItem> collectibleItems = new List<TeamPreviewItem>();

    public void UpdateTeamPreviewCallback(Action<CollectibleType> onSelectItemCallback, Action<CollectibleType> onRemoveCallback = null)
    {
        UpdateCollectiblesSelectCallback(onSelectItemCallback, onRemoveCallback);
    }

    public void UpdateTeam(CollectibleType selectedCollectible = CollectibleType.None)
    {
        List<CollectibleType> currentTeam = CollectibleManager.Instance.GetCurrentTeam();

        for (int i = 0; i < collectibleItems.Count; i++)
        {
            CollectibleType type = CollectibleType.None;

            if (i < currentTeam.Count)
            {
                type = currentTeam[i];
            }

            bool isSelected = type != CollectibleType.None && type == selectedCollectible;
            collectibleItems[i].Setup(type, isSelected);
        }
    }

    public void OpenTeamSelection()
    {
        CanvasManager.Instance.OpenMenu(Menu.TeamPreview);
        AudioManager.Instance.Play("Button");
    }

    private void UpdateCollectiblesSelectCallback(Action<CollectibleType> onSelectItemCallback, Action<CollectibleType> onRemoveCallback)
    {
        for (int i = 0; i < collectibleItems.Count; i++)
        {
            collectibleItems[i].UpdateSelectionCallback(onSelectItemCallback);

            if (onRemoveCallback != null)
            {
                collectibleItems[i].EnableRemoveFromTeam(onRemoveCallback);
            }
        }
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onUpdateTeam, OnUpdateTeam);
        UpdateTeam();
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onUpdateTeam, OnUpdateTeam);
    }

    private void OnUpdateTeam(IGameEvent gameEvent)
    {
        UpdateTeam();
    }
}
