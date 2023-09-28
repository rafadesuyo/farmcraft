using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldPopupView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private TMP_Text totalGoldText;

    //Getters
    public override Menu Type => Menu.GoldPopup;

    public void UpdateGold()
    {
        int gold = PlayerProgress.TotalGold;

        totalGoldText.text = gold.ToString();
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        UpdateVariables();
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    private void UpdateVariables()
    {
        UpdateGold();
    }

    private void ResetVariables()
    {
        totalGoldText.text = string.Empty;
    }
}
