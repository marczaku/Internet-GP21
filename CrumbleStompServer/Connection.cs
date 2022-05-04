using System.Net.Sockets;
using System.Text.Json;
using CrumbleStompServer.Messages;

namespace CrumbleStompServer;

/// <summary>
/// Contains all Code for one player's Connection.
/// Use <see cref="SendMessage{T}"/> to send a Message to this Player.
/// The class waits for incoming Messages on a separate thread. 
/// </summary>
public class Connection
{
    private readonly CrumbleStompMatch _match;
    private readonly PlayerInfo _playerInfo;
    private readonly StreamWriter streamWriter;

    private readonly JsonSerializerOptions options = new()
    {
        IncludeFields = true
    };

    public Connection(TcpClient client, CrumbleStompMatch match, PlayerInfo playerInfo)
    {
        _match = match;
        _playerInfo = playerInfo;
        this.Client = client;
        this.streamWriter = new StreamWriter(client.GetStream());
        new Thread(ReadPlayer).Start();
    }

    private TcpClient Client { get; }

    public void SendMessage<T>(T message)
    {
        streamWriter.WriteLine(JsonSerializer.Serialize(message, options));
        streamWriter.Flush();
    }

    public class Message
    {
        public string name;
    }
    
    public class Message<T> : Message
    {
        public T value;
    }
    
    void ReadPlayer()
    {
        var streamReader = new StreamReader(Client.GetStream());
        var options = new JsonSerializerOptions()
        {
            IncludeFields = true
        };
        while (true)
        {
            // block the reading thread until a whole line of information has arrived.
            string? json = streamReader.ReadLine();
            if (_playerInfo.ready)
            {
                // then, it should be a CookieClickMessage
                // TODO: increase cookies by one _playerInfo.cookies++;
            }
            else
            {
                // if the player is not ready, yet, we expect a LoginMessage.
                var loginMessage = JsonSerializer.Deserialize<LoginMessage>(json, options);
                Console.WriteLine($"[#{_match.Id}] Player '{loginMessage.playerName}' logged in.");
                _playerInfo.name = loginMessage.playerName;
                _playerInfo.ready = true;
            }
            
            _match.DistributeMatchInfo();
        }
    }
}