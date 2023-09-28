using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class TeamPreviewView : MenuView
{
    [SerializeField] private TeamPreviewUI teamPreview = null;

    [Header("Abilities")]
    [SerializeField] private TeamPreviewAbilitySection activeSection = null;
    [SerializeField] private TeamPreviewAbilitySection passiveSection = null;

    [Header("Selection")]
    [SerializeField] private TextMeshProUGUI sectionTitleText = null;
    [SerializeField] private TeamSelectionUI teamSelectionUI = null;

    private bool isSelectionOpen = false;

    public override Menu Type => Menu.TeamPreview;

    // Used by UI
    public void CloseView()
    {
        if (isSelectionOpen)
        {
            CloseSelectionUI();
        }
        else
        {
            CanvasManager.Instance.ReturnMenu();
        }
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        CloseSelectionUI();

        TrySelectCollectible();
        UpdateTeamUI();
    }

    private void TrySelectCollectible()
    {
        var currentTeam = CollectibleManager.Instance.GetCurrentTeam();
        if (currentTeam.Count > 0)
        {
            SelectCollectible(currentTeam[0]);
        }
        else
        {
            DeselectCollectible();
        }
    }

    private void UpdateTeamUI(CollectibleType type = CollectibleType.None)
    {
        EventsManager.Publish(EventsManager.onUpdateTeam);
        teamPreview.UpdateTeam(type);
        teamPreview.UpdateTeamPreviewCallback(SelectCollectible, RemoveCollectibleFromTeam);
    }

    private void SelectCollectible(CollectibleType newSelection)
    {
        if (newSelection == CollectibleType.None)
        {
            CollectibleManager.Instance.RemoveCollectibleFromCurrentTeam(newSelection);
            OpenSelectionUI();
            return;
        }
        else
        {
            if (CollectibleManager.Instance.CanAddCollectibleToCurrentTeam(newSelection))
            {
                CollectibleManager.Instance.AddCollectibleToCurrentTeam(newSelection);
            }

            CloseSelectionUI();
        }

        ShowCollectibleSelected(newSelection);
    }

    private void OpenSelectionUI()
    {
        isSelectionOpen = true;
        sectionTitleText.text = "Collectibles";
        teamSelectionUI.gameObject.SetActive(true);
        teamSelectionUI.Setup(SelectCollectible);
    }

    private void CloseSelectionUI()
    {
        isSelectionOpen = false;
        sectionTitleText.text = "Team";
        teamSelectionUI.gameObject.SetActive(false);
    }

    private void ShowCollectibleSelected(CollectibleType type)
    {
        Collectible collectible = CollectibleManager.Instance.GetCollectibleByType(type);
        UpdateTeamUI(type);
        UpdateAbilityItems(collectible.CollectibleAbilities);
    }

    private void UpdateAbilityItems(List<CollectibleAbility> selectedCollectibleAbilities)
    {
        if (selectedCollectibleAbilities == null)
        {
            activeSection.gameObject.SetActive(false);
            passiveSection.gameObject.SetActive(false);

            return;
        }

        var activeAbilities = selectedCollectibleAbilities.Where(ability => !ability.AbilityDataSO.IsPassiveUse).ToList();
        var passiveAbilities = selectedCollectibleAbilities.Except(activeAbilities).ToList();

        if (activeAbilities == null)
        {
            activeSection.gameObject.SetActive(false);
        }
        else
        {
            activeSection.gameObject.SetActive(true);
            activeSection.Setup(activeAbilities);
        }

        if (passiveAbilities == null)
        {
            passiveSection.gameObject.SetActive(false);
        }
        else
        {
            passiveSection.gameObject.SetActive(true);
            passiveSection.Setup(passiveAbilities);
        }
    }

    private void RemoveCollectibleFromTeam(CollectibleType type)
    {
        CollectibleManager.Instance.RemoveCollectibleFromCurrentTeam(type);
        DeselectCollectible();

        UpdateTeamUI(type);
        UpdateAbilityItems(null);
        AudioManager.Instance.Play("Button");
    }

    private void DeselectCollectible()
    {
        activeSection.gameObject.SetActive(false);
        passiveSection.gameObject.SetActive(false);
    }
}
