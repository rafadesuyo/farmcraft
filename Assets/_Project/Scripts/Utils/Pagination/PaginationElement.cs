using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[Serializable]
public class ElementState
{
    public Sprite backgroundImage = null;
}

[Serializable]
public class PaginationElement
{
    [Header("States")]
    [SerializeField] private ElementState disabledState = null;
    [SerializeField] private ElementState enabledState = null;

    [Header("UI Elements")]
    [SerializeField] private Button elementButton = null;
    [SerializeField] private Image elementButtonImage = null;
    [SerializeField] private GameObject tabContainer = null;

    [Header("Events")]
    [SerializeField] private UnityEvent onTabOpen = null;
    [SerializeField] private UnityEvent onTabClose = null;

    public void Setup(Action onOpenCallback = null, Action onCloseCallback = null)
    {
        onTabOpen.AddListener(() => onOpenCallback?.Invoke());
        onTabClose.AddListener(() => onCloseCallback?.Invoke());
        elementButton.onClick.AddListener(Open);

        Close();
    }

    public void Open()
    {
        EnableElement();
        tabContainer.SetActive(true);
        onTabOpen?.Invoke();
    }

    public void Close()
    {
        tabContainer.SetActive(false);
        onTabClose?.Invoke();
        DisableElement();
    }

    private void EnableElement()
    {
        UpdateElement(enabledState);
        elementButton.interactable = false;
    }

    private void DisableElement()
    {
        UpdateElement(disabledState);
        elementButton.interactable = true;
    }

    private void UpdateElement(ElementState state)
    {
        if (state.backgroundImage != null)
        {
            elementButtonImage.sprite = state.backgroundImage;
        }
    }
}
