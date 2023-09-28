using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionReviewScreensController : BaseQuestionScreensController
{
    [SerializeField] GameObject[] warningSubScreens;

    public static event Action OnReviewAQuestionScreenSelected;

    public override void Start()
    {
        base.Start();
        SetWarningSubScreen(0);
    }

    public void SetWarningSubScreen(int index)
    {
        if (warningSubScreens.Length == 0)
        {
            return;
        }

        for (int i = 0; i < warningSubScreens.Length; i++)
        {
            warningSubScreens[i].SetActive(i == index);
        }
    }

    public override void HideWarning()
    {
        if (warningScreen.activeSelf)
        {
            SetWarningSubScreen(0);
            warningScreen.SetActive(false);
        }
    }

    public void GoToReviewAQuestionScreen()
    {
        ShowScreen(0);
        OnReviewAQuestionScreenSelected?.Invoke();
    }

}
