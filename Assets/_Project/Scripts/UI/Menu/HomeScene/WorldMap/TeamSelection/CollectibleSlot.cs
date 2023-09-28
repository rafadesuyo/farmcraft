using UnityEngine;
using UnityEngine.UI;

public class CollectibleSlot : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private ButtonTab buttonTab;

    [Space(10)]

    [SerializeField] private Image collectibleImage;
    [SerializeField] private Image addCollectibleSymbol;

    //Events
    public delegate void CollectibleSlotEvent(CollectibleSlot collectibleSlot);

    public event CollectibleSlotEvent OnCollectibleSlotSelected;

    //Variables
    private Collectible currentCollectible;
    private CollectibleSO currentCollectibleData;

    private int index;

    //Getters
    public Collectible CurrentCollectible => currentCollectible;
    public int Index => index;

    public void Init(int index)
    {
        this.index = index;

        buttonTab.OnButtonClicked.AddListener(SelectSlot);
    }

    public void SetCollectible(Collectible collectible, CollectibleSO collectibleSO)
    {
        currentCollectible = collectible;
        currentCollectibleData = collectibleSO;

        UpdateVariables();

        collectibleImage.gameObject.SetActive(true);
        addCollectibleSymbol.gameObject.SetActive(false);
    }

    public void EmptyCollectible()
    {
        currentCollectible = null;
        currentCollectibleData = null;

        ResetVariables();

        collectibleImage.gameObject.SetActive(false);
        addCollectibleSymbol.gameObject.SetActive(true);
    }

    private void UpdateVariables()
    {
        collectibleImage.sprite = currentCollectibleData.Icon;
    }

    public void ResetVariables()
    {
        collectibleImage.sprite = null;
    }

    public void SelectSlot(int _)
    {
        OnCollectibleSlotSelected?.Invoke(this);
    }

    public void Selected(bool value)
    {
        buttonTab.ButtonSelected(value);
    }
}
