using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToggler : MonoBehaviour
{
    private Button _button;
    public GameObject[] ToggleOn;
    public GameObject[] ToggleOff;

    private void Awake()
    {
        try
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(ToggleGameObjects);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void ToggleGameObjects()
    {
        foreach (GameObject gameObject in ToggleOn)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }
        foreach (GameObject gameObject in ToggleOff)
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }
}