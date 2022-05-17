using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CrumbleStompShared.CrumbleStompShared.Interfaces;
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
        private readonly ILogger _logger;
        private readonly IJson _json;
        private readonly Dictionary<string, PlayerData> _playerDatas = new ();

        public PlayerDataBase(ILogger logger, IJson json)
        {
            _logger = logger;
            _json = json;
            if (File.Exists("players.json"))
            {
                var jsonText = File.ReadAllText("players.json");
                _playerDatas = json.Deserialize<Dictionary<string, PlayerData>>(jsonText);
                _logger.Log($"Found existing database with {_playerDatas.Count} player entries.");
            }
            else
            {
                _logger.Log("Found no existing database. Creating new one.");
            }
        }
        
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

        public void UpdatePlayer(PlayerData playerData)
        {
            _playerDatas[playerData.name] = playerData;
            Save();
        }

        private void Save()
        {
            var json = _json.Serialize(_playerDatas);
            File.WriteAllText("players.json", json);
        }
    }
}