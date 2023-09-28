using Newtonsoft.Json;

public enum LeaderboardType
{
    OverallRanking = 0,
    Stage = 1
}

public class LeaderboardManager : LocalSingleton<LeaderboardManager>
{
    private Leaderboard leaderboard = null;
    public Leaderboard Leaderboard => leaderboard;

    public void FetchLeaderboards(string leaderboardType = "OverallRanking")
    {
        string user = PlayerProgress.SaveState.playerInfo.username;
        ServerManager.Instance.SendGetRequest(ServerURL.GetLeaderboardUrl(user, leaderboardType), OnGetLeaderboard);
    }

    public void CreateUserScoreEntry()
    {
        string user = PlayerProgress.SaveState.playerInfo.username;
        ServerManager.Instance.SendPostRequest(ServerURL.GetCreateScoreUrl(user));
    }

    public void UpdateUserScore(int newScore, string leaderboardType = "OverallRanking")
    {
        string user = PlayerProgress.SaveState.playerInfo.username;
        ServerManager.Instance.SendPutRequest(ServerURL.GetUpdateScoreUrl(user, leaderboardType, newScore), OnGetLeaderboard, null, true);
    }

    public LeaderboardEntry GetUserScoreEntry()
    {
        LeaderboardEntry userEntry = Leaderboard.scoreEntryList.Find(score => string.Equals(score.username, PlayerProgress.SaveState.playerInfo.username));

        // not updated by server yet (server delay/cache)
        if (userEntry == null)
        {
            userEntry = new LeaderboardEntry();
            userEntry.username = PlayerProgress.Username;
            userEntry.score = 0;
        }

        return userEntry;
    }

    private void OnGetLeaderboard(string leaderboardData)
    {
        leaderboard = JsonConvert.DeserializeObject<Leaderboard>(leaderboardData);
        EventsManager.Publish(EventsManager.onFetchLeaderboard);
    }
}
