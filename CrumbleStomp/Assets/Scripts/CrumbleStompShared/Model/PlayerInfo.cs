using System;

namespace CrumbleStompShared.Model
{
    /// <summary>
    /// Contains all important information for one Player of a Match.
    /// </summary>
    [Serializable]
    public class PlayerInfo
    {
        public bool ready;
        public PlayerData data;
    }
}