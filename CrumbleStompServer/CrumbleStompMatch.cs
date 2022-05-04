using System.Net.Sockets;
using CrumbleStompServer.Messages;

namespace CrumbleStompServer;

/// <summary>
/// Holds the Match Info for a PVP Match.
/// As well as both player's connections.
/// </summary>
public class CrumbleStompMatch
{
    private static int id;
    public int Id { get; }
    private Connection? Red { get; set; }
    private Connection? Blue { get; set; }

    private readonly MatchInfo matchInfo = new();

    public CrumbleStompMatch()
    {
        Id = ++id;
    }

    public void InitRed(TcpClient client)
    {
        Red = new Connection(client, this, matchInfo.red);
    }
    
    public void InitBlue(TcpClient client)
    {
        Blue = new Connection(client, this, matchInfo.blue);
    }

    /// <summary>
    /// Helper Method to synchronize all players.
    /// </summary>
    public void DistributeMatchInfo()
    {
        var message = new MatchInfoMessage
        {
            matchInfo = this.matchInfo
        };
        Red?.SendMessage(message);
        Blue?.SendMessage(message);
    }
    
    /// <summary>
    /// Main Game Loop
    /// </summary>
    public void Start()
    {
        while (true)
        {
            if (!matchInfo.started)
            {
                if (matchInfo.blue.ready && matchInfo.red.ready)
                {
                    // start game
                    Console.WriteLine("Start Game!");
                    matchInfo.started = true;
                    DistributeMatchInfo();
                }
            }
            else
            {
                // TODO: check for winner (e.g. 10.0000 cookies)
            }
        }
    }
}