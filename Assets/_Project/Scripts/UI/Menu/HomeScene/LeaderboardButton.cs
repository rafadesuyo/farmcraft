using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        CanvasManager.Instance.OpenMenu(Menu.Leaderboard);
        AudioManager.Instance.Play("Button");
    }
}