using CrumbleStompShared.Model;
using CrumbleStompShared.Networking;

namespace CrumbleStompShared.Messages
{
    /// <summary>
    /// Distributed from Server to Clients to keep them synchronized.
    /// </summary>
    [System.Serializable]
    public class MatchInfoMessage : MessageBase
    {
        public MatchInfo matchInfo;
    }
}