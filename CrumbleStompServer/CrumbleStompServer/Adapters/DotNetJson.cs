using System;
using System.Text.Json;
using CrumbleStompShared.CrumbleStompShared.Interfaces;

namespace CrumbleStompServer
{
    public class DotNetJson : IJson
    {
        private readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true
        };
        
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public object Deserialize(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, options);
        }

        public string Serialize<T>(T data)
        {
            return JsonSerializer.Serialize(data, options);
        }
    }
}