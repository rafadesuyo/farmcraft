using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ReviewReasonButton : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Color selectedBackgroundColor;
    [SerializeField] private Color deselectedBackgroundColor;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] private Color selectedTextsColor;
    [SerializeField] private Color deselectedTextsColor;

    private bool isSelected = false;
    public bool IsSelected => isSelected;

    public static event Action OnReasonPicked;

    private void OnEnable()
    {
        TurnOffSelection();
    }

    public void TurnOffSelection()
    {
        isSelected = false;
        UpdateButtonColors();
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        UpdateButtonColors();

        OnReasonPicked?.Invoke();
    }

    private void UpdateButtonColors()
    {
        if (isSelected)
        {
            buttonBackground.color = selectedBackgroundColor;
            titleText.color = selectedTextsColor;
            bodyText.color = selectedTextsColor;
        }
        else
        {
            buttonBackground.color = deselectedBackgroundColor;
            titleText.color = deselectedTextsColor;
            bodyText.color = deselectedTextsColor;
        }
    }


}
