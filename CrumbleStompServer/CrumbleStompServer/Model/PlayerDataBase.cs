using System.Collections.Generic;
using CrumbleStompShared.Model;

namespace CrumbleStompServer.Model
{
    /// <summary>
    /// Simple Database
    /// It can store Entities (Data Objects that are Identifiable, that have an ID)
    /// They are compared by ID (e.g. Player("Marc", 200) == Player("Marc", 300), because the id (the name) are the same)
    /// Value Objects are compared by Value (e.g. Vector(2,2) == Vector(2,2), because all fields are the same)
    ///
    /// It can also retrieve Entities
    /// It-Memory Database (therefore stored in RAM)
    /// And RAM is non-persistent
    /// Therefore, this Database is non-persistent
    /// </summary>
    public class PlayerDataBase
    {
        private readonly Dictionary<string, PlayerData> _playerDatas = new ();

        public PlayerData GetOrCreatePlayer(string playerName)
        {
            if (!_playerDatas.TryGetValue(playerName, out var data))
            {
                data = new PlayerData
                {
                    name = playerName,
                    cookies = 0
                };
                _playerDatas[playerName] = data;
            }

            return data;
        }
    }
}