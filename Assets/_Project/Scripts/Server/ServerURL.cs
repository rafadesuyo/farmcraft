public class ServerURL
{
    //OUR API
    //Base URL
    private const string baseUrl = "https://dreamquizapi.ocarinastudio.com/api/";

    //Leaderboard
    private const string leaderdoardQuery = "leaderboard?";
    private const string scoreQuery = "score/";

    // Feedback
    private const string createFeedbackQuery = "feedback/create";

    //Leaderboard
    public static string GetLeaderboardUrl(string user, string leaderboardType)
    {
        return $"{baseUrl}{leaderdoardQuery}username={user}&scoreType={leaderboardType}";
    }

    public static string GetCreateScoreUrl(string user)
    {
        return $"{baseUrl}{scoreQuery}create?username={user}";
    }

    public static string GetUpdateScoreUrl(string user, string leaderboardType, int points)
    {
        return $"{baseUrl}{scoreQuery}update?username={user}&scoreType={leaderboardType}&points={points}";
    }

    public static string GetScoreCreateUrl(string user)
    {
        return $"{baseUrl}{scoreQuery}create?username={user}";
    }

    // Feedback
    public static string GetCreateFeedbackUrl()
    {
        return $"{baseUrl}{createFeedbackQuery}";
    }
}
