using System;

namespace CrumbleStompShared.Model
{
    /// <summary>
    /// Contains all important information for one Match.
    /// </summary>
    [Serializable]
    public class MatchInfo
    {
        public bool started;
        public PlayerInfo red = new PlayerInfo();
        public PlayerInfo blue = new PlayerInfo();
    }
}