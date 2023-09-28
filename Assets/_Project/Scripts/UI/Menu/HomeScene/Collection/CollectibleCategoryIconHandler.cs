using UnityEngine;
using UnityEngine.UI;

public class CollectibleCategoryIconHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image icon;
    [SerializeField] private Image backgroundImage;

    public void SetupCategory(QuizCategory category)
    {
        icon.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(category);
        backgroundImage.color = ProjectAssetsDatabase.Instance.GetCategoryColor(category);
    }
}
