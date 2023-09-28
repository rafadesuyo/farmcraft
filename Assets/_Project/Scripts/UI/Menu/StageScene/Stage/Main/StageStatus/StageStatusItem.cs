using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageStatusItem : MonoBehaviour
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private Image selectedEffect = null;
    [SerializeField] private TextMeshProUGUI valueTxt = null;
    [SerializeField] private TextMeshProUGUI titleTxt = null;
    [SerializeField] private TextMeshProUGUI descriptionTxt = null;
    [SerializeField] private RectTransform descriptionContainer = null;

    public void UpdateItem(Sprite icon, string value, string title, string description)
    {
        iconImg.sprite = icon;
        valueTxt.text = value;
        titleTxt.text = title;
        descriptionTxt.text = description; 
    }

    public void OnButtonPressed()
    {
        descriptionContainer.gameObject.SetActive(true);
        selectedEffect.gameObject.SetActive(true);

        EventsManager.Publish(EventsManager.onSelectStageStatusIcon);
    }

    public void OnButtonRelesead()
    {
        descriptionContainer.gameObject.SetActive(false);
        selectedEffect.gameObject.SetActive(false);

        EventsManager.Publish(EventsManager.onDeselectStageStatusIcon);
    }

    private void OnEnable()
    {
        OnButtonRelesead();
    }
}
