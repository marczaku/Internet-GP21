using System.IO;
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
    /// It is persistent and saves each player to its individual File.
    /// </summary>
    public class PlayerDataBase
    {
        private readonly IJson _json;

        public PlayerDataBase(IJson json)
        {
            if (!Directory.Exists("players"))
                Directory.CreateDirectory("players");
            _json = json;
        }

        static string GetFilePath(string playerName) => $"players/{playerName}.json";
        
        public PlayerData GetOrCreatePlayer(string playerName)
        {
            // if the player exists, load him from the file
            if (File.Exists(GetFilePath(playerName)))
            {
                var jsonText = File.ReadAllText(GetFilePath(playerName));
                return _json.Deserialize<PlayerData>(jsonText);
            }
            
            // else, create a new player
            var data = new PlayerData
            {
                name = playerName,
                cookies = 0
            };
            // save him to disk
            UpdatePlayer(data);
            // and return him
            return data;
        }

        public void UpdatePlayer(PlayerData playerData)
        {
            File.WriteAllText($"players/{playerData.name}.json", _json.Serialize(playerData));
        }
    }
}