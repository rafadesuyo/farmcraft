using DreamQuiz;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuizAnswerEventArgs
{
    public Guid QuestionID { get; private set; }
    public string AnswerText { get; private set; }
    public QuizCategory Category { get; private set; }
    public QuizDifficulty.Level NodeDifficulty { get; private set; }
    public bool IsCorrect { get; private set; }
    public float Duration { get; private set; }

    public QuizAnswerEventArgs(Guid questionId, string answerText, QuizCategory category, QuizDifficulty.Level nodeDifficulty, bool isCorrect, float duration)
    {
        QuestionID = questionId;
        AnswerText = answerText;
        Category = category;
        NodeDifficulty = nodeDifficulty;
        IsCorrect = isCorrect;
        Duration = duration;
    }
}

public enum QuizState
{
    None = 0,
    Starting,
    QuestionReady,
    WaitingForAnswer,
    CorrectAnswer,
    WrongAnswer,
    TimedOut,
    Passed,
    Canceled
}

public class QuizSystem : BaseStageSystem
{
    [SerializeField] private bool hasTimeout = true;
    [SerializeField] private float quizTimeout = 15f;
    private float currentQuizTimeout = 0;
    private int targetQuestionCount = 1;
    private int currentQuestionCount = 0;

    private QuizState currentQuizState = QuizState.None;
    private QuestionModel currentQuestion = null;
    private QuizCategory currentQuizCategory = QuizCategory.Random;
    private QuizDifficulty.Level currentQuizDifficulty = QuizDifficulty.Level.Easy;
    private bool isTimeoutSuspended = true;

    public int TargetQuestionCount
    {
        get
        {
            return targetQuestionCount;
        }
    }

    public int CurrentQuestionCount
    {
        get
        {
            return currentQuestionCount;
        }
    }

    public QuestionModel CurrentQuizQuestion
    {
        get
        {
            return currentQuestion;
        }
    }

    public QuizState CurrentQuizState => currentQuizState;
    public bool IsTimeoutSuspended { get => isTimeoutSuspended; set => isTimeoutSuspended = value; }

    public event Action<QuizState> OnQuizStateChange;

    public event Action<QuestionModel> OnQuestionShow;

    public event Action OpenQuiz;

    public event Action OnQuestionAnswerRight;

    public event Action ResetAnswers;

    public event Action<QuestionModel> OnQuestionUpdate;

    public event Action<QuizAnswerEventArgs> OnAnswer;

    public event Action<float> OnTimeoutChange;

    private void Update()
    {
        WaitingForAnswerUpdate();
    }

    protected override void RegisterSystem()
    {
        bool isRegistered = StageSystemLocator.RegisterSystem(this);
        IsReady = isRegistered;
    }

    protected override void UnregisterSystem()
    {
        StageSystemLocator.UnregisterSystem<QuizSystem>();
    }

    private void SetQuizState(QuizState quizState)
    {
        currentQuizState = quizState;

        OnQuizStateChange?.Invoke(currentQuizState);
    }

    public void StartQuiz(int targetQuestionCount, QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty)
    {
        if (currentQuizState != QuizState.None)
        {
            return;
        }

        SetQuizState(QuizState.Starting);

        currentQuestionCount = 0;
        this.targetQuestionCount = targetQuestionCount;

        SetupNewQuizQuestion(quizCategory, quizDifficulty);
        ShowQuestion();
    }

    private void SetupNewQuizQuestion(QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty)
    {
        currentQuizCategory = quizCategory;
        currentQuizDifficulty = quizDifficulty;
        currentQuestion = GetQuestion(quizCategory, quizDifficulty);
        SetQuizState(QuizState.QuestionReady);
    }

    private QuestionModel GetQuestion(QuizCategory category, QuizDifficulty.Level difficulty)
    {
        return QuestionDatabaseManager.Instance.GetQuestionFromDatabase(category, difficulty);
    }

    private void ShowQuestion()
    {
        OpenQuiz.Invoke();
        isTimeoutSuspended = true;
        currentQuizTimeout = quizTimeout;
        OnQuestionShow?.Invoke(currentQuestion);
        SetQuizState(QuizState.WaitingForAnswer);
    }

    public void SetupAndShowNextQuestion(QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty)
    {
        SetupNewQuizQuestion(quizCategory, quizDifficulty);
        ShowQuestion();
    }

    public void SetupAndShowNextQuestion()
    {
        SetupAndShowNextQuestion(currentQuizCategory, currentQuizDifficulty);
    }

    private void WaitingForAnswerUpdate()
    {
        if (!hasTimeout
            || currentQuizState != QuizState.WaitingForAnswer
            || isTimeoutSuspended)
        {
            return;
        }

        currentQuizTimeout -= Time.deltaTime;

        float timeRatio = Mathf.Clamp(currentQuizTimeout / quizTimeout, 0, 1);
        OnTimeoutChange?.Invoke(timeRatio);

        if (currentQuizTimeout < 0)
        {
            ResetAnswers.Invoke();
            TimeoutQuiz();
        }
    }

    public void AnswerQuestion(string answerText)
    {
        if (currentQuizState != QuizState.WaitingForAnswer)
        {
            return;
        }

        isTimeoutSuspended = true;

        bool isCorrect = currentQuestion.IsAnswerCorrect(answerText);

        OnAnswer?.Invoke(
            new QuizAnswerEventArgs(
                CurrentQuizQuestion.QuestionId,
                answerText,
                currentQuizCategory,
                currentQuizDifficulty,
                isCorrect,
                quizTimeout - currentQuizTimeout));

        if (isCorrect)
        {
            OnQuestionAnswerRight.Invoke();
            currentQuestionCount++;
            SetQuizState(QuizState.CorrectAnswer);
        }
        else
        {
            ResetAnswers.Invoke();
            SetQuizState(QuizState.WrongAnswer);
        }
    }

    public void FinishQuestion()
    {
        if (currentQuestionCount >= targetQuestionCount)
        {
            PassQuiz();
        }
        else
        {
            SetupAndShowNextQuestion();
        }
        ResetAnswers.Invoke();
    }

    private void TimeoutQuiz()
    {
        if (currentQuizState != QuizState.WaitingForAnswer)
        {
            return;
        }
        ResetAnswers.Invoke();
        SetQuizState(QuizState.TimedOut);
        SetupAndShowNextQuestion();
    }

    private void PassQuiz()
    {
        SetQuizState(QuizState.Passed);
        ResetAnswers.Invoke();
        EndQuiz();
    }

    public void CancelQuiz()
    {
        SetQuizState(QuizState.Canceled);
        EndQuiz();
    }

    public void EndQuiz()
    {
        SetQuizState(QuizState.None);
    }

    public void CutRandomWrongAnswer()
    {
        if (currentQuestion.Answers.Count < 2)
        {
            return;
        }

        var wrongAnswers = currentQuestion.Answers.Where(a => currentQuestion.IsAnswerCorrect(a) == false).ToList();
        var randomWrongAnswer = wrongAnswers[Random.Range(0, wrongAnswers.Count)];

        currentQuestion.Answers.Remove(randomWrongAnswer);

        OnQuestionUpdate?.Invoke(currentQuestion);
        ResetAnswers.Invoke();
    }
}