using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesView : MenuView
{
    [SerializeField] private TextMeshProUGUI collectibleNameTxt = null;
    [SerializeField] private TextMeshProUGUI collectibleCategoryTxt = null;
    [SerializeField] private Image collectibleCategoryImg = null;

    [Space(10)]

    [SerializeField] private AbilityListItem[] abilityItems = null;

    public override Menu Type => Menu.AbilitiesInfo;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        PopulateInfos(setupOptions.collectible);
    }

    private void PopulateInfos(Collectible collectible)
    {
        CollectibleSO collectibleData = collectible.Data;
        collectibleNameTxt.text = collectibleData.Name;
        collectibleCategoryTxt.text = collectibleData.Category.ToString();

        PopulateAbilities(collectible);
    }

    private void PopulateAbilities(Collectible collectible)
    {
        for (int i = 0; i < abilityItems.Length; i++)
        {
            if (i >= collectible.CollectibleAbilities.Count)
            {
                abilityItems[i].gameObject.SetActive(false);
                continue;
            }

            abilityItems[i].gameObject.SetActive(true);
            abilityItems[i].Setup(collectible.CollectibleAbilities[i].AbilityDataSO);
        }

        collectibleCategoryImg.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(collectible.Data.Category);
    }
}
