using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        this.Connection.PlayerName = playerName;
        this.Connection.SendMessage(new LoginMessage
        {
            playerName = playerName
        });
        this.Connection.MessageReceived += OnMessageReceived;
    }

    void OnMessageReceived(string json)
    {
        // TODO: implement client logic
        var matchInfo = JsonUtility.FromJson<MatchInfoMessage>(json);
        Debug.Log(json);
    }
}