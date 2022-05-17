using System;
using System.Net.Sockets;
using CrumbleStompServer.Model;
using CrumbleStompShared.Messages;
using CrumbleStompShared.Model;
using CrumbleStompShared.Networking;

namespace CrumbleStompServer.Networking
{
    /// <summary>
    /// Contains all Code for one player's Connection.
    /// Use <see cref="SendMessage{T}"/> to send a Message to this Player.
    /// The class waits for incoming Messages on a separate thread. 
    /// </summary>
    public class ClientConnection
    {
        private readonly CrumbleStompMatch _match;
        private readonly PlayerInfo _playerInfo;
        private readonly PlayerDataBase _playerDataBase;
        public Connection Connection { get; }

        public ClientConnection(TcpClient client, CrumbleStompMatch match, PlayerInfo playerInfo, PlayerDataBase playerDataBase)
        {
            Connection = new Connection(new ConsoleLogger(), new DotNetJson(), client);
            _match = match;
            _playerInfo = playerInfo;
            _playerDataBase = playerDataBase;
            Connection.Broker.Subscribe<LoginMessage>(OnLoginReceived);
            Connection.Broker.Subscribe<CollectCookieMessage>(OnCollectCookieReceived);
        }

        private void OnCollectCookieReceived(CollectCookieMessage collectCookie)
        {
            _playerInfo.data.cookies++;
            _match.DistributeMatchInfo();
        }

        void OnLoginReceived(LoginMessage loginMessage)
        {
            Console.WriteLine($"[#{_match.Id}] Player '{loginMessage.playerName}' logged in.");
            Connection.PlayerName = loginMessage.playerName;
            _playerInfo.data = _playerDataBase.GetOrCreatePlayer(loginMessage.playerName);
            _playerInfo.ready = true;
            _match.DistributeMatchInfo();
        }
    }
}