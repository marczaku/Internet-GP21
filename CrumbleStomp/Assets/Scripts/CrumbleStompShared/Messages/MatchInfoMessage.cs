using CrumbleStompShared.Model;

namespace CrumbleStompShared.Messages
{
    /// <summary>
    /// Distributed from Server to Clients to keep them synchronized.
    /// </summary>
    public class MatchInfoMessage
    {
        public MatchInfo matchInfo;
    }
}