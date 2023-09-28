using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DreamQuiz.Player;

public class StageView : MenuView
{
    [SerializeField] private StageSettingsUI stageSettingsUI = null;
    [SerializeField] private StageTeamUI collectiblesView = null;
    [SerializeField] private StageAbilityPreviewUI abilitiesPreview = null;
    [SerializeField] private NodeInfoUI nodeInfoView = null;

    [SerializeField] private CameraController cameraController;

    private bool setupWasMade = false;

    public override Menu Type => Menu.Stage;

    public void OpenSettingsWindow()
    {
        AudioManager.Instance.Play("Button");
        stageSettingsUI.gameObject.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        AudioManager.Instance.Play("Button");
        stageSettingsUI.gameObject.SetActive(false);
    }

    public void SetCameraToFollowPlayer()
    {
        cameraController.SetCameraMode(CameraController.CameraMode.FollowPlayer);
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        if (!setupWasMade)
        {
            SetupBehaviors();

            setupWasMade = true;
        }

        EventsManager.Publish(EventsManager.onOpenStageView);
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onSelectCollectibleAbility, OnSelectCollectibleAbility);
        EventsManager.AddListener(EventsManager.onDeselectCollectibleAbility, OnDeselectCollectibleAbility);
        EventsManager.AddListener(EventsManager.onDeselectNode, OnDeselectNode);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onSelectCollectibleAbility, OnSelectCollectibleAbility);
        EventsManager.RemoveListener(EventsManager.onDeselectCollectibleAbility, OnDeselectCollectibleAbility);
        EventsManager.RemoveListener(EventsManager.onDeselectNode, OnDeselectNode);
    }

    private void SetupBehaviors()
    {
        stageSettingsUI.UpdateMuteButton();
    }

    private void ShowCollectibleAbilities(List<CollectibleAbility> abilities)
    {
        abilitiesPreview.gameObject.SetActive(true);
        abilitiesPreview.Setup(abilities);
    }

    private void HideCollectibleAbilities()
    {
        abilitiesPreview.gameObject.SetActive(false);
    }

    private void OnSelectCollectibleAbility(IGameEvent gameEvent)
    {
        OnSelectCollectibleAbilityPreviewEvent previewEvent = ((OnSelectCollectibleAbilityPreviewEvent)gameEvent);
        ShowCollectibleAbilities(previewEvent.abilitiesToShow);
    }

    private void OnDeselectCollectibleAbility(IGameEvent gameEvent)
    {
        HideCollectibleAbilities();
    }

    private void OnDeselectNode(IGameEvent gameEvent)
    {
        nodeInfoView.gameObject.SetActive(false);
    }
}
