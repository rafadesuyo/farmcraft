using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrialStageFooterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI correctAnswersCountText;
    [SerializeField] Slider timeSlider;

    TimeCountdownSystem timeCountdownSystem;
    QuizSystem quizSystem;

    private void OnEnable()
    {
        if (quizSystem == null)
        {
            quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        }

        quizSystem.OnQuizStateChange += RefreshCorrectAnswersCountdown;
        quizSystem.OnQuizStateChange += SetUIVisibility;
    }

    private void OnDisable()
    {
        quizSystem.OnQuizStateChange -= RefreshCorrectAnswersCountdown;
        quizSystem.OnQuizStateChange -= SetUIVisibility;
    }

    void Start()
    {
        timeSlider.transform.parent.gameObject.SetActive(false);
        timeCountdownSystem = StageSystemLocator.GetSystem<TimeCountdownSystem>();
    }

    private void Update() 
    {
        timeSlider.value = timeCountdownSystem.CalculateTimeProportion();
    }

    public void SetUIVisibility(QuizState questionState)
    {
        timeSlider.transform.parent.gameObject.SetActive(questionState != QuizState.None);
    }

    //TEMPORARY TEXT
    public void RefreshCorrectAnswersCountdown(QuizState questionState)
    {
        correctAnswersCountText.text = "Correct answers: " + quizSystem.CurrentQuestionCount.ToString();
    }
}
