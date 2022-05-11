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
    public class ObjectHolder
    {
        public string type;

        public ObjectHolder(string type)
        {
            this.type = type;
        }
    }

    public class ObjectHolder<T> : ObjectHolder
    {
        public T obj;

        public ObjectHolder(T obj) : base(typeof(ObjectHolder<T>).FullName)
        {
            this.obj = obj;
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

        public void SendMessage<T>(T message)
        {
            streamWriter.WriteLine(m_Json.Serialize(new ObjectHolder<T>(message)));
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
                var holder = m_Json.Deserialize<ObjectHolder>(json);
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Select(assembly => assembly.GetType(holder.type))
                    .Single(type => type != null);
                var objectHolder = m_Json.Deserialize(json, type) as ObjectHolder;
                var listener = listeners[type];
                listener.DynamicInvoke(objectHolder);
            }
        }

        public void Subscribe<T>(Action<ObjectHolder<T>> onMessageReceived)
        {
            listeners[typeof(ObjectHolder<T>)] = onMessageReceived;
        }
    }
}
