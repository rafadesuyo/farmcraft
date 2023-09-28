using UnityEngine;

public class LeaderboardView : MenuView
{
    [SerializeField] private int maxEntriesToShow = 15;
    [SerializeField] private RectTransform leaderboardContainer = null;
    [SerializeField] private GameObject leaderboardPrefab = null;
    [SerializeField] private LeaderboardEntryItem playerEntryItem = null;

    public override Menu Type => Menu.Leaderboard;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        GenericPool.CreatePool<LeaderboardEntryItem>(leaderboardPrefab, leaderboardContainer);
        LeaderboardManager.Instance.FetchLeaderboards();
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onFetchLeaderboard, OnFetchLeaderboard);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onFetchLeaderboard, OnFetchLeaderboard);
    }

    private void PopulateLeaderboard()
    {
        SetupPlayerEntry();
        int currentPosition = 1;
        foreach (LeaderboardEntry entry in LeaderboardManager.Instance.Leaderboard.scoreEntryList)
        {
            if (currentPosition > maxEntriesToShow)
            {
                return;
            }

            LeaderboardEntryItem entryItem = GenericPool.GetItem<LeaderboardEntryItem>();
            entryItem.Setup(entry.username, currentPosition, entry.score);
            currentPosition++;
        }
    }

    private void SetupPlayerEntry()
    {
        LeaderboardEntry userEntry = LeaderboardManager.Instance.GetUserScoreEntry();
        playerEntryItem.Setup(userEntry.username, LeaderboardManager.Instance.Leaderboard.PlayerPosition, userEntry.score, true);
    }

    private void OnFetchLeaderboard(IGameEvent gameEvent)
    {
        PopulateLeaderboard();
    }
}
