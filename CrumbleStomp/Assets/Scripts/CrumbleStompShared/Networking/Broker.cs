using System;
using System.Collections.Generic;

namespace CrumbleStompShared.Networking
{
    public class Broker
    {
        private readonly Dictionary<Type, Delegate> listeners = new();
        public event Action<MessageBase> AnyMessageReceived;
        
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

        public void InvokeSubscribers(Type type, MessageBase data)
        {
            if (listeners.TryGetValue(type, out var listener))
            {
                listener.DynamicInvoke(data);
            }
            this.AnyMessageReceived?.Invoke(data);
        }
    }
}