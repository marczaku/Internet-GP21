using System;
using System.Net.Sockets;
using CrumbleStompServer.Interfaces;
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
        private readonly IDatabase<PlayerData> _playerDataBase;
        public Connection Connection { get; }

        public ClientConnection(TcpClient client, CrumbleStompMatch match, PlayerInfo playerInfo, IDatabase<PlayerData> playerDataBase)
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
            _playerDataBase.Update(_playerInfo.data.name, _playerInfo.data);
            _match.DistributeMatchInfo();
        }

        void OnLoginReceived(LoginMessage loginMessage)
        {
            Console.WriteLine($"[#{_match.Id}] Player '{loginMessage.playerName}' logged in.");
            Connection.PlayerName = loginMessage.playerName;
            _playerInfo.data = _playerDataBase.ReadOrCreate(loginMessage.playerName);
            _playerInfo.ready = true;
            _match.DistributeMatchInfo();
        }
    }
}