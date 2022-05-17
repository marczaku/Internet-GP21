using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CrumbleStompServer.Model;
using CrumbleStompShared.Messages;

namespace CrumbleStompServer
{
    /// <summary>
    /// Matchmaking class. It assigns newly connected players.
    /// To CrumbleStompMatch Instances.
    /// </summary>
    public static class Program
    {
        public class Player
        {
            public string name;
        }

        public class Enemy
        {
            public string name;
        }

        
        public static void Main()
        {
            // start listening to new connections on the given socket
            var tcpListener = new TcpListener(IPAddress.Any, 12244);
            tcpListener.Start();

            Console.WriteLine("Server listening on: "+tcpListener.LocalEndpoint);

            PlayerDataBase dataBase = new PlayerDataBase(new DotNetJson());
            CrumbleStompMatch match = null;

            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                // wait for a client to establish a connection and return that connection
                var tcpClient = tcpListener.AcceptTcpClient();

                Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} connected!");
    
                if (match == null)
                {
                    Console.WriteLine("Starting new Match for Player. Waiting for Second Player.");
                    match = new CrumbleStompMatch(dataBase);
                    match.InitRed(tcpClient);
                }
                else
                {
                    Console.WriteLine("Assigning Player to existing Match. Starting Match.");
                    match.InitBlue(tcpClient);
                    new Thread(match.Start).Start();
                    match = null;
                }
            }
        }
    }
}