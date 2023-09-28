using DreamQuiz;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileTab_PersonalInfo : ProfileTab
{
    //Components
    [Header("Components")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text emailText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timePlayingText;

    //Variables
    [Header("Default Variables")]
    [SerializeField][TextArea] private string deleteSaveText = "Are you sure you want to delete the save?\nAll your progress, gold and collectibles will be lost. This cannot be undone!";

    public override void UpdateVariables()
    {
        timePlayingText.text = $"{PlayerProgress.GameplayHours}h";
    }

    public override void ResetVariables()
    {
        usernameText.text = string.Empty;
        nameText.text = string.Empty;
        emailText.text = string.Empty;
        scoreText.text = string.Empty;
        timePlayingText.text = string.Empty;
    }

    public void DeleteSave()
    {
        PopupManager.Instance.Open(deleteSaveText, ConfirmDeleteSave);
    }

    private void ConfirmDeleteSave()
    {
        GameManager.Instance.DeleteSavedData();
        StageLoadManager.Instance.ReturnToInitialScene();
    }
}
