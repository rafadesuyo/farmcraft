using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleInfoView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private Image categoryImg = null;
    [SerializeField] private TMP_Text categoryTxt = null;
    [SerializeField] private TMP_Text nameTxt = null;
    [SerializeField] private TMP_Text shardCountTxt = null;
    [SerializeField] private TMP_Text descriptionTxt = null;
    [SerializeField] private Button abilitiesViewBtn = null;
    [SerializeField] private CollectibleLevelProgressHandler levelHandler;

    public override Menu Type => Menu.CollectibleInfo;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        if (setupOptions == null)
        {
            return;
        }

        // TODO: Review later, maybe it's worth it to change to Collectible object (if needed to show level, shards, etc)
        Collectible collectible = CollectibleManager.Instance.GetCollectibleByType(setupOptions.collectibleType);
        PopulateInfos(collectible);
    }

    private void PopulateInfos(Collectible collectible)
    {
        nameTxt.text = collectible.Data.Name;
        categoryImg.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(collectible.Data.Category);
        categoryTxt.text = $"{collectible.Data.Category}";
        descriptionTxt.text = collectible.Data.Description;
        shardCountTxt.text = $"{collectible.CurrentShards}/{collectible.ShardsToNextLevel}";
        levelHandler.SetupLevel(collectible.CurrentLevel);

        abilitiesViewBtn.onClick.RemoveAllListeners();
        abilitiesViewBtn.onClick.AddListener(() => ShowAbilites(collectible));
    }

    private void ShowAbilites(Collectible collectible)
    {
        CanvasManager.Instance.OpenMenu(Menu.AbilitiesInfo, new MenuSetupOptions(collectible));
    }
}