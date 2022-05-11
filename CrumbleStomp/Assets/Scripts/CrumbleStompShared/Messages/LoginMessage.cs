using CrumbleStompShared.Networking;

namespace CrumbleStompShared.Messages
{
    [System.Serializable]
    public class LoginMessage : MessageBase
    {
        public string playerName;
    }
}