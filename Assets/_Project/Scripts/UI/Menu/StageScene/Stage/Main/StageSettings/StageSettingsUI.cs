using DreamQuiz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSettingsUI : MonoBehaviour
{
    //Component
    [Header("Components")]
    [SerializeField] private Button muteBtn = null;
    [SerializeField] private TextMeshProUGUI muteText = null;
    [SerializeField] private Image muteIcon = null;

    [Header("Menus")]
    [SerializeField] private UIControllerAnimated confirmationToExitStageUI;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textMute = "Mute";
    [SerializeField] private string textUnmute = "Unmute";

    public void ToggleMuteAudio()
    {
        SoundManager.Instance.ToggleGlobalMuteState();
        UpdateMuteButton();
    }

    public void OpenConfirmationToExitStageUI()
    {
        confirmationToExitStageUI.OpenUI();

        AudioManager.Instance.Play("Button");
    }

    public void ExitStage()
    {
        StageManager.Instance.ReturnToWorld();
    }

    public void UpdateMuteButton()
    {
        if (SoundManager.Instance.IsGlobalAudioMuted == false)
        {
            muteText.text = textMute;
        }
        else
        {
            muteText.text = textUnmute;
        }

        muteIcon.gameObject.SetActive(SoundManager.Instance.IsGlobalAudioMuted);
    }
}
