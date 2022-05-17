using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using CrumbleStompShared.Messages;
using CrumbleStompShared.Networking;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    private static ServerConnection _instance;

    public Connection Connection;

    public Broker Broker { get; } = new ();

    private Queue<MessageBase> messageQueue = new();

    public static ServerConnection Instance
    {
        get
        {
            _instance ??= CreateInstance();
            return _instance;
        }
    }

    static ServerConnection CreateInstance()
    {
        var go = new GameObject("ServerConnection");
        DontDestroyOnLoad(go);
        var connection = go.AddComponent<ServerConnection>();
        return connection;
    }
    
    public void Connect(string playerName)
    {
        var client = new TcpClient();
        client.Connect(IPAddress.Loopback, 12244);
        this.Connection = new Connection(new UnityLogger(), new UnityJson(), client);
        this.Connection.Broker.AnyMessageReceived += OnAnyMessageReceived;
        this.Connection.PlayerName = playerName;
        this.Connection.SendMessage(new LoginMessage
        {
            playerName = playerName
        });
    }

    void OnAnyMessageReceived(MessageBase message)
    {
        messageQueue.Enqueue(message);
    }

    void Update()
    {
        while (messageQueue.Count > 0)
        {
            var message = messageQueue.Dequeue();
            this.Broker.InvokeSubscribers(message.GetType(), message);
        }
    }
}