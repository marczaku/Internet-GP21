using System.Net;
using System.Net.Sockets;
using CrumbleStompShared.Messages;
using CrumbleStompShared.Networking;
using UnityEngine;

public class ServerConnection
{
    private static ServerConnection _instance;

    public Connection Connection;

    public static ServerConnection Instance
    {
        get
        {
            _instance ??= new ServerConnection();
            return _instance;
        }
    }
    
    public void Connect(string playerName)
    {
        var client = new TcpClient();
        client.Connect(IPAddress.Loopback, 12244);
        this.Connection = new Connection(new UnityLogger(), new UnityJson(), client);
        this.Connection.Subscribe<MatchInfoMessage>(OnMessageReceived);
        this.Connection.PlayerName = playerName;
        this.Connection.SendMessage(new LoginMessage
        {
            playerName = playerName
        });
    }

    void OnMessageReceived(MatchInfoMessage matchInfo)
    {
        Debug.Log(matchInfo);
    }
}