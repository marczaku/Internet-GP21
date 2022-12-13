using System;
using System.IO;
using CrumbleStompServer.Interfaces;
using CrumbleStompShared.CrumbleStompShared.Interfaces;
using CrumbleStompShared.Model;
using Npgsql;

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
    /// CRUD Operations
    /// Create - Read - Update - Delete
    /// </summary>
    public class PlayerDataBase : IDatabase<PlayerData>
    {
        private readonly IJson _json;

        public PlayerDataBase(IJson json)
        {
            var connectionString = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Username = "agario_server",
                Password = "agr_srvr",
                Database = "agario"
            }.ToString();
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            var sql = "SELECT * from agario.users";
            using var cmd = new NpgsqlCommand(sql, con);
            var users = cmd.ExecuteReader();
            Console.WriteLine($"PostgreSQL version: {users}");
        }

        static string GetFilePath(string playerName) => $"players/{playerName}.json";

        public PlayerData Create(string id)
        {
            throw new System.NotImplementedException();
        }

        public PlayerData ReadOrCreate(string id)
        {
            // if the player exists, load him from the file
            if (File.Exists(GetFilePath(id)))
            {
                var jsonText = File.ReadAllText(GetFilePath(id));
                return _json.Deserialize<PlayerData>(jsonText);
            }

            // else, create a new player
            var data = new PlayerData
            {
                name = id,
                cookies = 0
            };
            // save him to disk
            Update(id, data);
            // and return him
            return data;
        }

        public PlayerData Read(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(string id, PlayerData data)
        {
            File.WriteAllText($"players/{id}.json", _json.Serialize(data));
        }

        public void Delete(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}