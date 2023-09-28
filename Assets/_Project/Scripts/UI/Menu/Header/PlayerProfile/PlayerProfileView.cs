using DreamQuiz;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ProfileTabs
{
    PersonalInfo = 0,
    Achievements = 1,
    Progress = 2
}

[System.Serializable]
public class ProfileTabPair
{
    public ProfileTabs tab;
    public GameObject container;
    public GameObject tabIcon;
    public Sprite activeBackgroundImage;
    public Sprite inactiveBackgroundImage;
}

public class PlayerProfileView : MenuView
{
    [SerializeField] private List<ProfileTabPair> profileTabs = new List<ProfileTabPair>();

    [Space(10)]

    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private Button deleteSaveButton;

    [SerializeField][TextArea] private string deleteSaveText = "Are you sure you want to delete the save?\nAll your progress, gold and collectibles will be lost. This cannot be undone!";

    private GameObject currentTab = null;

    public override Menu Type => Menu.Profile;

    public void ChangeTab(int index)
    {
        ChangeTab((ProfileTabs)index);
        AudioManager.Instance.Play("Button");
    }

    public void DeleteSave()
    {
        PopupManager.Instance.Open(deleteSaveText, ConfirmDeleteSave);
    }

    public override void Initialize()
    {
        versionText.text = $"Version {Application.version}";
        deleteSaveButton.onClick.AddListener(DeleteSave);
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        ChangeTab(ProfileTabs.Achievements);
    }

    private void ChangeTab(ProfileTabs tab)
    {
        if (currentTab != null)
        {
            currentTab.SetActive(false);
            SetTabIconState(currentTab, false);
        }

        ProfileTabPair selectedTabPair = profileTabs.Find(pair => pair.tab == tab);
        currentTab = selectedTabPair.container;

        currentTab.SetActive(true);
        SetTabIconState(currentTab, true);
    }

    private void ConfirmDeleteSave()
    {
        GameManager.Instance.DeleteSavedData();
        StageLoadManager.Instance.ReturnToInitialScene();
    }

    private void SetTabIconState(GameObject tabContainer, bool isActive)
    {
        foreach (ProfileTabPair tabPair in profileTabs)
        {
            if (tabPair.container == tabContainer)
            {
                Image backgroundImage = tabPair.tabIcon.GetComponent<Image>();
                backgroundImage.sprite = isActive ? tabPair.activeBackgroundImage : tabPair.inactiveBackgroundImage;
            }
            else
            {
                tabPair.tabIcon.GetComponent<Image>().sprite = tabPair.inactiveBackgroundImage;
            }
        }
    }
}
