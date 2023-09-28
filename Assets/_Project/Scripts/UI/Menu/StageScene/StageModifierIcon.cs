using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageModifierIcon : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image modifierIcon;
    [SerializeField] private TMP_Text modifierValue;

    [Space(10)]

    [SerializeField] private RectTransform infoPanel;

    public void SetModifierIcon(Sprite image)
    {
        modifierIcon.sprite = image;
    }

    public void SetModifierValue(string value)
    {
        modifierValue.text = value;
    }

    public void InfoPanelVisible(bool value)
    {
        infoPanel.gameObject.SetActive(value);
    }
}
