using UnityEngine;
using UnityEngine.Events;

public class FindOrUpgradeButtonHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private RectTransform findShardsButton;
    [SerializeField] private UpgradeCollectibleButton upgradeCollectibleButton;
    [SerializeField] private RectTransform maxLevelButton;

    //Events
    [Header("Events")]
    [SerializeField] private UnityEvent onFindShardsButtonPressed = new UnityEvent();
    [SerializeField] private UnityEvent onUpgradeCollectibleButtonPressed = new UnityEvent();

    //Getters
    public UnityEvent OnFindShardsButtonPressed => onFindShardsButtonPressed;
    public UnityEvent OnUpgradeCollectibleButtonPressed => onUpgradeCollectibleButtonPressed;

    public void Setup(Collectible collectible)
    {
        maxLevelButton.gameObject.SetActive(collectible.IsMaxLevel);

        if (collectible.IsMaxLevel == false)
        {
            findShardsButton.gameObject.SetActive(!collectible.HasEnoughShardsToLevelUp);
            upgradeCollectibleButton.gameObject.SetActive(collectible.HasEnoughShardsToLevelUp);

            upgradeCollectibleButton.Setup(200); //TODO: get the gold value to upgrade a collectible. Link: https://ocarinastudios.atlassian.net/browse/DQG-1795?atlOrigin=eyJpIjoiYTUzMzU1YTk2NWMxNDg4ZmE2MWQzNTlkNDVlYTZhNmMiLCJwIjoiaiJ9
        }
        else
        {
            findShardsButton.gameObject.SetActive(false);
            upgradeCollectibleButton.gameObject.SetActive(false);
        }
    }

    public void PressFindShardsButton()
    {
        onFindShardsButtonPressed?.Invoke();
    }

    public void PressUpgradeCollectibleButton()
    {
        onUpgradeCollectibleButtonPressed?.Invoke();
    }
}
