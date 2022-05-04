using System.Net;
using System.Net.Sockets;

namespace CrumbleStompServer;

public static class Program
{
    public static void Main()
    {
        // start listening to new connections on the given socket
        var tcpListener = new TcpListener(IPAddress.Any, 12244);
        tcpListener.Start();

        Console.WriteLine("Server listening on: "+tcpListener.LocalEndpoint);

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
                match = new CrumbleStompMatch();
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