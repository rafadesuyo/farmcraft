using UnityEngine;

public class DifficultyChangeUI : MonoBehaviour
{
    [SerializeField] private GameObject difficultyContainer = null;
    private int clicksNeededToOpen = 5;
    private int currentClickCount = 0;

    public void ChangeToNormal()
    {
        GameDifficultyHandler.Instance.ChangeDifficulty(GameDifficulty.Normal);
        CloseDifficultyUI();
    }

    public void ChangeToHard()
    {
        GameDifficultyHandler.Instance.ChangeDifficulty(GameDifficulty.Hard);
        CloseDifficultyUI();
    }

    public void OnHiddenClick()
    {
        currentClickCount++;

        if (currentClickCount >= clicksNeededToOpen)
        {
            difficultyContainer.SetActive(true);
        }
    }

    public void CloseDifficultyUI()
    {
        difficultyContainer.SetActive(false);
    }

    private void OnEnable()
    {
        currentClickCount = 0;
    }
}
