using System.IO;
using System.Net.Sockets;
using Messages;
using UnityEngine;

public class Connection
{
    private static Connection _instance;
    private StreamWriter streamWriter;
    private StreamReader streamReader;

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
        this.streamReader = new StreamReader(client.GetStream());
        SendMessage(new LoginMessage
        {
            playerName = playerName
        });
    }

    public TcpClient Client { get; private set; }
    public string PlayerName { get; private set; }

    public void SendMessage<T>(T message)
    {
        streamWriter.WriteLine(JsonUtility.ToJson(message));
        streamWriter.Flush();
    }
}
