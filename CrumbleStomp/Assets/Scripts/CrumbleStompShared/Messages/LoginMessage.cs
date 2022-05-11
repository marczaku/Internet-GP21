namespace CrumbleStompShared.Messages
{
    [System.Serializable]
    public class LoginMessage
    {
        public string playerName;
        public string id;
        public int score;
    }
}