using System.Net.Sockets;
using System.Text.Json;
using CrumbleStompServer.Messages;

namespace CrumbleStompServer;

public class CrumbleStompMatch
{
    private static int id;
    public int Id { get; }
    public TcpClient Red { get; private set; }
    public TcpClient Blue { get; private set; }

    public MatchInfo matchInfo = new MatchInfo();

    public CrumbleStompMatch()
    {
        Id = ++id;
    }

    public void InitRed(TcpClient client)
    {
        Red = client;
        
        // start reading thread
        new Thread(() =>
        {
            var streamReader = new StreamReader(client.GetStream());
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true
            };
            while (true)
            {
                // block the reading thread until a whole line of information has arrived.
                string? json = streamReader.ReadLine();
                
                if (json == null)
                    continue;
                    
                var loginMessage = JsonSerializer.Deserialize<LoginMessage>(json, options);
                Console.WriteLine($"[#{Id}] Red player '{loginMessage.playerName}' logged in.");
                matchInfo.red = new PlayerInfo()
                {
                    name = loginMessage.playerName
                };
            }
        }).Start();
    }
    
    public void InitBlue(TcpClient client)
    {
        Blue = client;
        
        // start reading thread
        new Thread(() =>
        {
            var streamReader = new StreamReader(client.GetStream());
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true
            };
            while (true)
            {
                // block the reading thread until a whole line of information has arrived.
                string json = streamReader.ReadLine();
                var loginMessage = JsonSerializer.Deserialize<LoginMessage>(json, options);
                Console.WriteLine($"[#{Id}] Blue player '{loginMessage.playerName}' logged in.");
                matchInfo.blue = new PlayerInfo()
                {
                    name = loginMessage.playerName
                };
            }
        }).Start();
    }
    
    public void Start()
    {
        while (true)
        {
            if (!matchInfo.started)
            {
                if (matchInfo.blue != null && matchInfo.red != null)
                {
                    // start game
                    Console.WriteLine("Start Game!");
                    matchInfo.started = true;
                }
            }
        }
    }
}