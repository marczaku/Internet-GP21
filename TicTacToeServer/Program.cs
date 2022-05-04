using System.Net;
using System.Net.Sockets;
using System.Numerics;
using TcpExample;

// start listening to new connections on the given socket
var tcpListener = new TcpListener(IPAddress.Any, 12244);
tcpListener.Start();

Console.WriteLine("Server listening on: "+tcpListener.LocalEndpoint);

TicTacToe ticTacToe = null;

while (true)
{
    Console.WriteLine("Waiting for connection...");
    // wait for a client to establish a connection and return that connection
    var tcpClient = tcpListener.AcceptTcpClient();

    Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} connected!");
    
    if (ticTacToe == null)
    {
        Console.WriteLine("Starting new Match for Player. Waiting for Second Player.");
        ticTacToe = new TicTacToe
        {
            x = tcpClient
        };
    }
    else
    {
        Console.WriteLine("Assigning Player to existing Match. Starting Match.");
        ticTacToe.o = tcpClient;
        new Thread(ticTacToe.Start).Start();
        ticTacToe = null;
    }
}