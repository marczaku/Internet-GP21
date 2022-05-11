using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using CrumbleStompShared.CrumbleStompShared.Interfaces;
using CrumbleStompShared.Messages;

namespace CrumbleStompShared.Networking
{
    public class MessageBase
    {
        public string type;

        public MessageBase()
        {
            this.type = GetType().FullName;
        }
    }
    
    public class Connection
    {
        private readonly ILogger m_Logger;
        private readonly IJson m_Json;
        private readonly StreamWriter streamWriter;
        private readonly Dictionary<Type, Delegate> listeners = new();

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

        public void SendMessage<TMessage>(TMessage message)
        where TMessage : MessageBase
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
                if(json == null)
                    continue;
                var holder = m_Json.Deserialize<MessageBase>(json);
                
                if (holder == null)
                {
                    m_Logger.Log($"Invalid message received: {json}");
                    continue;
                }
                
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Select(assembly => assembly.GetType(holder.type))
                    .SingleOrDefault(type => type != null);
                
                if (type == null)
                {
                    m_Logger.Log($"Unsupported Message of Type {holder.type} received. Ignoring.");
                    continue;
                }
                
                var objectHolder = m_Json.Deserialize(json, type) as MessageBase;
                if (listeners.TryGetValue(type, out var listener))
                {
                    listener.DynamicInvoke(objectHolder);
                }
            }
        }

        public void Subscribe<TMessage>(Action<TMessage> onMessageReceived)
            where TMessage : MessageBase
        {
            if (listeners.TryGetValue(typeof(TMessage), out var del))
                listeners[typeof(TMessage)] = Delegate.Combine(del, onMessageReceived);
            else
                listeners[typeof(TMessage)] = onMessageReceived;
        }

        public void Unsubscribe<TMessage>(Action<TMessage> onMessageReceived)
            where TMessage : MessageBase
        {
            if (listeners.TryGetValue(typeof(TMessage), out var del))
                listeners[typeof(TMessage)] = Delegate.Remove(del, onMessageReceived);
        }
    }
}
