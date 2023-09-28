using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Daily Missions/Missions Types/Answer Questions")]
public class DailyMissionSO_AnswerQuestions : DailyMissionSO
{
    //Variables
    [Header("Parameters")]
    [SerializeField] private int numberOfQuestionsToAnswerMin;
    [SerializeField] private int numberOfQuestionsToAnswerMax;

    [Space(10)]

    [SerializeField] private bool answerNeedsToBeCorrect;
    [SerializeField] private bool questionNeedsToBeOfSpecificCategory;

    //TODO: update mission with a variable of the mode in which the question was answered (Any, Story Mode or Trial Mode). Link: https://ocarinastudios.atlassian.net/browse/DQG-1970?atlOrigin=eyJpIjoiNzU3YjQzZDE4YjJhNGQxYjllNGVkNWQwODBkZWRjY2EiLCJwIjoiaiJ9

    [Space(10)]

    [Tooltip("The text that will be replaced with the number of questions to answer of the mission.")]
    [SerializeField] private string textToReplaceWithNumberOfQuestionsToAnswer = "{value}";

    [Tooltip("The text that will be replaced with the question category of the mission.\nThe category will only be considered in the mission if \"questionNeedsToBeOfSpecificCategory\" is true.")]
    [SerializeField] private string textToReplaceWithCategory = "{category}";

    public override string GetDescription(DailyMission dailyMission)
    {
        int numberOfQuestionsToAnswer = dailyMission.RequiredMissionProgress[0].targetValue;

        QuizCategory missionQuizCategory = (QuizCategory)dailyMission.CurrentMissionProgress[0].targetType;
        string categoryName = ProjectAssetsDatabase.Instance.GetCategoryName(missionQuizCategory);

        StringBuilder descriptionText = new StringBuilder(description);

        descriptionText.Replace(textToReplaceWithNumberOfQuestionsToAnswer, numberOfQuestionsToAnswer.ToString());
        descriptionText.Replace(textToReplaceWithCategory, categoryName);

        return descriptionText.ToString();
    }

    public override string GetMissionEventName()
    {
        return EventsManager.onQuestionIsAnswered;
    }

    public override DailyMission.DailyMissionProgress[] GetInitialMissionProgress()
    {
        DailyMission.DailyMissionProgress initialMissionProgress = GetMissionProgressOfRandomCategory();

        return new DailyMission.DailyMissionProgress[] { initialMissionProgress };
    }

    public override DailyMission.DailyMissionProgress[] GetRequiredMissionProgress(DailyMission.DailyMissionProgress[] initialMissionProgress)
    {
        int numberOfQuestionsToAnswer = Random.Range(numberOfQuestionsToAnswerMin, numberOfQuestionsToAnswerMax + 1);

        DailyMission.DailyMissionProgress requiredMissionProgress = new DailyMission.DailyMissionProgress(initialMissionProgress[0].targetType, numberOfQuestionsToAnswer);

        return new DailyMission.DailyMissionProgress[] { requiredMissionProgress };
    }

    public override void HandleMissionUpdate(DailyMission.DailyMissionProgress[] currentMissionProgress, IGameEvent gameEvent)
    {
        OnQuestionIsAnsweredEvent onQuestionIsAnsweredEvent = (OnQuestionIsAnsweredEvent)gameEvent;

#if UNITY_EDITOR
        if(showDebugs == true)
        {
            QuizCategory missionQuizCategory = (QuizCategory)currentMissionProgress[0].targetType;

            Debug.Log($"HandleMissionUpdate called for Daily Mission of Type \"{this}\".\nAnswer Category: {onQuestionIsAnsweredEvent.category}, Is Correct: {onQuestionIsAnsweredEvent.isAnswerCorrect}, Mission Quiz Category: {missionQuizCategory}.");
        }
#endif

        if(answerNeedsToBeCorrect == true && onQuestionIsAnsweredEvent.isAnswerCorrect == false)
        {
            return;
        }

        if (questionNeedsToBeOfSpecificCategory == true)
        {
            QuizCategory missionQuizCategory = (QuizCategory)currentMissionProgress[0].targetType;

            if(onQuestionIsAnsweredEvent.category != missionQuizCategory)
            {
                return;
            }
        }

        currentMissionProgress[0].targetValue++;

        GameManager.Instance.SaveGame();

#if UNITY_EDITOR
        if (showDebugs == true)
        {
            Debug.Log($"Daily Mission of Type \"{this}\" was updated.\nProgress: {currentMissionProgress[0].targetValue}.");
        }
#endif
    }

    public override void GoToMission()
    {
        CanvasManager.Instance.OpenMenu(Menu.WorldMap, null, true);
    }

    private DailyMission.DailyMissionProgress GetMissionProgressOfRandomCategory()
    {
        QuizCategory[] quizCategories = QuizCategoryMaps.GetAllCategories();

        int questionCategory = (int)quizCategories[Random.Range(0, quizCategories.Length)];

        return new DailyMission.DailyMissionProgress(questionCategory, 0);
    }
}
