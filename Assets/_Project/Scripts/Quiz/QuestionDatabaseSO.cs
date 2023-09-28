using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Question Database", menuName = "Question/Database")]
public class QuestionDatabaseSO : ScriptableObject
{
    [SerializeField] private bool removeQuestionOnGet = false;
    [SerializeField] private List<QuestionModel> questions = new List<QuestionModel>();

    public IReadOnlyList<QuestionModel> Questions => questions;

    public void AddQuestions(List<QuestionModel> questions)
    {
        this.questions.AddRange(questions);
    }

    public void ResetQuestions()
    {
        questions.Clear();
    }

    public QuestionModel GetRandomQuestionByCategoryAndDifficulty(QuizCategory category, QuizDifficulty.Level difficulty)
    {
        QuestionModel question = null;

        List<QuestionModel> questionList = questions.Where(q => q.Category == category && q.Difficulty == difficulty).ToList();

        if (questionList != null && questionList.Count > 0)
        {
            question = questionList[Random.Range(0, questionList.Count)];

            if (removeQuestionOnGet)
            {
                questions.Remove(question);
            }
        }

        return question;
    }

    public int GetQuestionCountByCategoryAndDifficulty(QuizCategory category, QuizDifficulty.Level difficulty)
    {
        return questions.Where(q => q.Category == category && q.Difficulty == difficulty).Count();
    }
}
