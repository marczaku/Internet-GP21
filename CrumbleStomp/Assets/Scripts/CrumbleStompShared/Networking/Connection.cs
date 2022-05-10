using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using CrumbleStompShared.CrumbleStompShared.Interfaces;

namespace CrumbleStompShared.Networking
{
    public class Connection
    {
        public event Action<string> MessageReceived;
        
        private readonly ILogger m_Logger;
        private readonly IJson m_Json;
        private readonly StreamWriter streamWriter;

        private TcpClient Client { get; }
        public string PlayerName { get; set; }

        public Connection(ILogger logger, IJson json, TcpClient client)
        {
            m_Logger = logger;
            m_Json = json;
            Client = client;
            this.streamWriter = new StreamWriter(client.GetStream());
            new Thread(ReceiveMessages).Start();
        }

        public void SendMessage<T>(T message)
        {
            streamWriter.WriteLine(m_Json.Serialize(message));
            streamWriter.Flush();
        }
    
        void ReceiveMessages()
        {
            var streamReader = new StreamReader(Client.GetStream());
            while (true)
            {
                // block the reading thread until a whole line of information has arrived.
                string? json = streamReader.ReadLine();
                if(json != null)
                    MessageReceived?.Invoke(json);
            }
        }
    }
}
