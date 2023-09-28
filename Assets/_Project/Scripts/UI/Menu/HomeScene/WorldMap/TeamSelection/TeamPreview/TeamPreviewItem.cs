using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TeamPreviewItem : MonoBehaviour
{
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private Image collectibleImage = null;

    [SerializeField] private Button addCollectibleBtn = null;
    [SerializeField] private Button collectibleBtn = null;
    [SerializeField] private Button removeCollectibleBtn = null;

    [SerializeField] private TextMeshProUGUI nameTxt = null;

    private bool canBeRemoved = false;

    private CollectibleType collectibleType;
    public CollectibleType CollectibleType => collectibleType;

    public void Setup(CollectibleType newCollectible, bool isSelected)
    {
        collectibleType = newCollectible;
        UpdateIcon();

        if (isSelected)
        {
            OnSelectCollectible();
        }
        else
        {
            OnDeselectCollectible();
        }
    }

    public void UpdateSelectionCallback(Action<CollectibleType> onSelectItemCallback = null)
    {
        addCollectibleBtn.onClick.RemoveAllListeners();
        addCollectibleBtn.onClick.AddListener(() => onSelectItemCallback(collectibleType));
        addCollectibleBtn.onClick.AddListener(OnSelectCollectible);

        collectibleBtn.onClick.RemoveAllListeners();
        collectibleBtn.onClick.AddListener(() => onSelectItemCallback(collectibleType));
        collectibleBtn.onClick.AddListener(OnSelectCollectible);
    }

    public void EnableRemoveFromTeam(Action<CollectibleType> onRemoveItemCallback)
    {
        canBeRemoved = true;
        removeCollectibleBtn.onClick.RemoveAllListeners();
        removeCollectibleBtn.onClick.AddListener(() => onRemoveItemCallback(collectibleType));
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onSelectNewTeamMember, OnSelectNewTeamMember);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onSelectNewTeamMember, OnSelectNewTeamMember);
    }

    private void UpdateIcon()
    {
        var collectibleData = CollectibleManager.Instance.GetCollectibleDataByType(collectibleType);

        if (collectibleType == CollectibleType.None)
        {
            ResetIcon();

            if (nameTxt != null)
            {
                nameTxt.text = "";
            }
        }
        else
        {
            addCollectibleBtn?.gameObject.SetActive(false);
            collectibleBtn?.gameObject.SetActive(true);

            if (canBeRemoved)
            {
                removeCollectibleBtn.gameObject.SetActive(true);
            }

            collectibleImage.sprite = collectibleData.Icon;

            if (nameTxt != null)
            {
                nameTxt.text = collectibleData.Name;
            }
        }
    }

    private void OnSelectCollectible()
    {
        if (backgroundImage == null)
        {
            return;
        }

        EventsManager.Publish(EventsManager.onSelectNewTeamMember);

        if (collectibleType != CollectibleType.None)
        {
            backgroundImage.enabled = true;
        }
    }

    private void OnDeselectCollectible()
    {
        if (backgroundImage == null)
        {
            return;
        }

        backgroundImage.enabled = false;
    }

    private void ResetIcon()
    {
        addCollectibleBtn?.gameObject.SetActive(true);
        collectibleBtn?.gameObject.SetActive(false);

        if (canBeRemoved)
        {
            removeCollectibleBtn.gameObject.SetActive(false);
        }
    }

    private void OnSelectNewTeamMember(IGameEvent gameEvent)
    {
        OnDeselectCollectible();
    }
}
