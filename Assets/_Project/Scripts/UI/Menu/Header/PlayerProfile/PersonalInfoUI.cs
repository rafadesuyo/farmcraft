using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalInfoUI : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Button editButton;
    [SerializeField] private Button confirmEditButton;

    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Image usernameInputFieldImage;
    [SerializeField] private TextMeshProUGUI usernameAlreadyExistsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playTimeText;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private Color normalInputTextColor;
    [SerializeField] private Color warningInputTextColor;

    [Space(10)]

    [SerializeField] private Color normalInputColor;
    [SerializeField] private Color warningInputColor;

    private void OnEnable()
    {
        UpdateVariables();

        usernameInputField.onValueChanged.AddListener(OnUsernameInputFieldValueChanged);
    }

    private void OnDisable()
    {
        EditModeActive(false);

        usernameInputField.onValueChanged.RemoveListener(OnUsernameInputFieldValueChanged);
    }

    public void EditModeActive(bool active)
    {
        editButton.gameObject.SetActive(!active);
        confirmEditButton.gameObject.SetActive(active);

        usernameText.gameObject.SetActive(!active);
        usernameInputField.gameObject.SetActive(active);

        ResetUsernameInputField();
    }

    public void ConfirmEdit()
    {
        TryCreateUsername();
    }

    private void UpdateVariables()
    {
        usernameText.text = PlayerProgress.Username;
        scoreText.text = PlayerProgress.GetOverallRanking().ToString();
        
        TimeSpan totalPlayTime = PlayerProgress.TotalPlayTime;
        string formattedTime = $"{totalPlayTime.Hours:D2}:{totalPlayTime.Minutes:D2}:{totalPlayTime.Seconds:D2}";
        playTimeText.text = formattedTime;
    }

    private void ResetUsernameInputField()
    {
        usernameInputField.text = PlayerProgress.Username;

        HideUsernameAlreadyExistsFeedback();
    }

    private void HideUsernameAlreadyExistsFeedback()
    {
        usernameInputField.textComponent.color = normalInputTextColor;
        usernameInputFieldImage.color = normalInputColor;
        usernameAlreadyExistsText.gameObject.SetActive(false);
    }

    private void ShowUsernameAlreadyExistsFeedback()
    {
        usernameInputField.textComponent.color = warningInputTextColor;
        usernameInputFieldImage.color = warningInputColor;
        usernameAlreadyExistsText.gameObject.SetActive(true);
    }

    private void OnUsernameInputFieldValueChanged(string value)
    {
        HideUsernameAlreadyExistsFeedback();
    }

    private void TryCreateUsername()
    {
        string userToCreate = usernameInputField.text;
        ServerManager.Instance.SendPostRequest(ServerURL.GetScoreCreateUrl(userToCreate), "", (data) => OnCreateUsernameSuccess(userToCreate), OnCreateUsernameFailed, true);
    }

    private void OnCreateUsernameSuccess(string newUsername)
    {
        PlayerProgress.UpdateUsername(newUsername);

        GameManager.Instance.SaveGame();

        EditModeActive(false);
        UpdateVariables();
    }

    private void OnCreateUsernameFailed()
    {
        ShowUsernameAlreadyExistsFeedback();
    }
}
