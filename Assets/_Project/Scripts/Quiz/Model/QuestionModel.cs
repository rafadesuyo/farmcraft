using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionModel
{
    [SerializeField] private Guid questionId;
    [SerializeField] private string title;
    [SerializeField] private List<string> answers;
    [SerializeField] private QuizCategory quizCategory;
    [SerializeField] private QuizDifficulty.Level quizDifficulty;
    [SerializeField] private int correctIndex;
    [SerializeField] private string hint;

    public Guid QuestionId => questionId;
    public string Title => title;
    public List<string> Answers => answers;
    public QuizCategory Category => quizCategory;
    public QuizDifficulty.Level Difficulty => quizDifficulty;
    public int CorrectIndex => correctIndex;
    public string Hint => hint;

    public QuestionModel(Guid questionId, string title, List<string> answers, QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty, int correctIndex, string hint)
    {
        this.questionId = questionId;
        this.title = title;
        this.answers = answers;
        this.quizCategory = quizCategory;
        this.quizDifficulty = quizDifficulty;
        this.correctIndex = correctIndex;
        this.hint = hint;
    }

    public bool IsAnswerCorrect(int index)
    {
        return CorrectIndex == index;
    }

    public bool IsAnswerCorrect(string answerText)
    {
        int index = answers.IndexOf(answerText);
        return index == CorrectIndex;
    }
}
