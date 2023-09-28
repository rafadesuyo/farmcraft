using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameModeIcon : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private GameModeIconButton gameModeIconButton;
    [SerializeField] private RectTransform moveToLeftButton;
    [SerializeField] private RectTransform moveToRightButton;

    [Space(10)]

    [SerializeField] private RectTransform infoPanel;

    private RectTransform iconsHolder;

    //Events
    [Header("Events")]
    [SerializeField] private UnityEvent onIconSelected = new UnityEvent();

    public event Action OnIconMovedToLeft;
    public event Action OnIconMovedToRight;
    public event Action OnIconEndDrag;

    //Getters
    public RectTransform MoveToLeftButton => moveToLeftButton;
    public RectTransform MoveToRightButton => moveToRightButton;
    public RectTransform IconsHolder => iconsHolder;
    public UnityEvent OnIconSelected => onIconSelected;

    public void Init(RectTransform iconsHolder)
    {
        this.iconsHolder = iconsHolder;

        gameModeIconButton.Init(this);

        InfoPanelActive(false);
    }

    public void SelectIcon()
    {
        onIconSelected?.Invoke();
    }

    public void IconMovedToLeft()
    {
        OnIconMovedToLeft?.Invoke();
        AudioManager.Instance.Play("Button");
    }

    public void IconMovedToRight()
    {
        OnIconMovedToRight?.Invoke();
        AudioManager.Instance.Play("Button");
    }

    public void IconEndDrag()
    {
        OnIconEndDrag?.Invoke();
    }

    public void InfoPanelActive(bool value)
    {
        infoPanel.gameObject.SetActive(value);
    }
}
