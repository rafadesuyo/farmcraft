using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoItem : MonoBehaviour
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TextMeshProUGUI descriptionTxt = null;

    public void Setup(Sprite icon, string description)
    {
        iconImg.sprite = icon;
        descriptionTxt.text = description;
    }

    public void EnableIcon()
    {
        if (iconImg != null)
        {
            iconImg.enabled = true;
        }
    }

    public void DisableIcon()
    {
        if (iconImg != null)
        {
            iconImg.enabled = false;
        }
    }

    public void EnableText()
    {
        if (descriptionTxt != null)
        {
            descriptionTxt.enabled = true;
        }
    }

    public void DisableText()
    {
        if (descriptionTxt != null)
        {
            descriptionTxt.enabled = false;
        }
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}