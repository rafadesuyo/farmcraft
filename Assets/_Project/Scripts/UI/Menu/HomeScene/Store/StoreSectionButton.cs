using UnityEngine;
using UnityEngine.UI;

public class StoreSectionButton : MonoBehaviour
{
    [SerializeField] private Button sectionButton = null;
    [SerializeField] private Image background = null;
    [SerializeField] private Image icon = null;

    [Space(10)]
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private ItemType section;
    public ItemType Section => section;

    public void Setup(ItemType newSection, Sprite newIcon, System.Action<StoreSectionButton> onChooseSectionCallback)
    {
        section = newSection;
        icon.sprite = newIcon;
        sectionButton.onClick.RemoveAllListeners();
        sectionButton.onClick.AddListener(() => OnSelect(onChooseSectionCallback));

        ResetButton();
    }

    public void ResetButton()
    {
        background.sprite = unselectedSprite;
        sectionButton.interactable = true;
    }

    public void ForceSelect()
    {
        sectionButton.onClick?.Invoke();
    }

    private void OnSelect(System.Action<StoreSectionButton> onChooseSectionCallback)
    {
        onChooseSectionCallback?.Invoke(this);
        background.sprite = selectedSprite;
        sectionButton.interactable = false;
        AudioManager.Instance.Play("Button");
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
