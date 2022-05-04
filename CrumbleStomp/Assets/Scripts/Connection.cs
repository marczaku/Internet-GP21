using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Messages;
using UnityEngine;

public class Connection
{
    public event Action<MatchInfoMessage> matchInfoMessageReceived;
    private static Connection _instance;
    private StreamWriter streamWriter;

    public static Connection Instance
    {
        get
        {
            _instance ??= new Connection();
            return _instance;
        }
    }

    public void Init(TcpClient client, string playerName)
    {
        this.Client = client;
        this.PlayerName = playerName;
        this.streamWriter = new StreamWriter(client.GetStream());
        new Thread(ReadPlayer).Start();
        SendMessage(new LoginMessage
        {
            playerName = playerName
        });
    }

    private TcpClient Client { get; set; }
    private string PlayerName { get; set; }

    public void SendMessage<T>(T message)
    {
        streamWriter.WriteLine(JsonUtility.ToJson(message));
        streamWriter.Flush();
    }
    
    void ReadPlayer()
    {
        var streamReader = new StreamReader(Client.GetStream());
        while (true)
        {
            // block the reading thread until a whole line of information has arrived.
            string? json = streamReader.ReadLine();
            var matchInfo = JsonUtility.FromJson<MatchInfoMessage>(json);
            matchInfoMessageReceived?.Invoke(matchInfo);
            Debug.Log(json);
        }
    }
}
