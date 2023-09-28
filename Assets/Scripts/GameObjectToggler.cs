using UnityEngine;
using UnityEngine.UI;

public class GameObjectToggler : MonoBehaviour
{
    public Button Button;
    public GameObject[] ActivateObject;
    public GameObject[] DeactivateObject;

    public void ToggleObjects()
    {
        foreach (GameObject go in ActivateObject)
        {
            go.SetActive(!go.activeInHierarchy);
        }

        foreach (GameObject go in DeactivateObject)
        {
            go.SetActive(!go.activeInHierarchy);
        }
    }

    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => ToggleObjects());
    }
}