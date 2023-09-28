using System.Collections.Generic;
using System;

[Serializable]
public class Leaderboard
{
    public string scoreType = "";
    public List<LeaderboardEntry> scoreEntryList = new List<LeaderboardEntry>();
    public int playerPositionIndex = 0;
    public string createdOn = "";
    public DateTime CreatedOn
    {
        get
        {
            return DateTime.Parse(createdOn);
        }
    }

    public int PlayerPosition
    {
        get
        {
            return playerPositionIndex + 1;
        }
    }
}
