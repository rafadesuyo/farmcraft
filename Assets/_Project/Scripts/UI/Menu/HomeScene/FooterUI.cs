using UnityEngine;

public class FooterUI : MonoBehaviour
{
    public void BackToHome()
    {
        CanvasManager.Instance.OpenMenu(Menu.Home, null, true);
        AudioManager.Instance.Play("Button");
    }

    public void OpenStore()
    {
        CanvasManager.Instance.OpenMenu(Menu.Store, null, true);
    }

    public void OpenCollection()
    {
        CanvasManager.Instance.OpenMenu(Menu.Collection, null, true);
    }

    public void OpenLeaderboard()
    {
        CanvasManager.Instance.OpenMenu(Menu.Leaderboard);
        AudioManager.Instance.Play("Button");
    }
}
