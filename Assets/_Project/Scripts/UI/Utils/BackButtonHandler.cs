using Unity.VisualScripting;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class BackButtonHandler : MonoBehaviour
{
    private string buttonSFX = "Button";

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(CanvasManager.Instance.ReturnMenu);
        GetComponent<Button>().onClick.AddListener(OnPointerDown);
    }

    public void OnPointerDown()
    {
        AudioManager.Instance.Play(buttonSFX);
    }

}
