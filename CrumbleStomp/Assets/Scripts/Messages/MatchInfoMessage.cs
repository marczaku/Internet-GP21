namespace Messages
{
    /// <summary>
    /// Distributed from Server to Clients to keep them synchronized.
    /// </summary>
    public class MatchInfoMessage
    {
        public MatchInfo matchInfo;
    }

    /// <summary>
    /// Contains all important information for one Match.
    /// </summary>
    public class MatchInfo
    {
        public bool started;
        public PlayerInfo red = new PlayerInfo();
        public PlayerInfo blue = new PlayerInfo();
    }

    /// <summary>
    /// Contains all important information for one Player of a Match.
    /// </summary>
    public class PlayerInfo
    {
        public bool ready;
        public string name;
        public int cookies;
    }
}