using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTxt = null;
    [SerializeField] private Button confirmButton = null;

    public void Setup(string title, System.Action confirmCallback = null)
    {
        titleTxt.text = title;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => confirmCallback?.Invoke());
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
