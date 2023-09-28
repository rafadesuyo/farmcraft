using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseQuestionScreensController : MonoBehaviour
{
    [SerializeField] protected GameObject[] screens;
    [SerializeField] protected GameObject warningScreen;

    public virtual void Start()
    {
        HideAllScreens();
    }

    public void HideAllScreens()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(false);
        }

        HideWarning();
    }

    public void ShowScreen(int index)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(i == index);
        }

        HideWarning();
    }

    public void ShowWarning()
    {
        if (warningScreen == null)
        {
            return;
        }

        warningScreen.SetActive(true);
    }

    public virtual void HideWarning()
    {
        if (warningScreen == null)
        {
            return;
        }

        if (warningScreen.activeSelf)
        {
            warningScreen.SetActive(false);
        }
    }
}
