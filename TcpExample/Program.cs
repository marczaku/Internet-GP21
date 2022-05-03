using System.Net;
using System.Net.Sockets;

// start listening to new connections on the given socket
var tcpListener = new TcpListener(IPAddress.Loopback, 12244);
tcpListener.Start();

while (true)
{
    Console.WriteLine("Waiting for connection...");
    // wait for a client to establish a connection and return that connection
    var tcpClient = tcpListener.AcceptTcpClient();
    new Thread(() =>
    {
        Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} connected!");
        // get the stream, so we can read and write data to and from it
        var stream = tcpClient.GetStream();
        
        // helper class to make reading text from a stream easier
        var streamReader = new StreamReader(stream);
        // helper class to make writing text to a stream easier
        var streamWriter = new StreamWriter(stream);
        streamWriter.AutoFlush = true;
        
        streamWriter.WriteLine("Welcome, what's your name?");
        
        // read all information currently buffered on the socket's stream
        var name = streamReader.ReadLine();
        // print that information to the console
        Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} sent: {name}");

        streamWriter.WriteLine("Welcome to the server, " + name);
        Console.WriteLine($"Closing the connection to {tcpClient.Client.RemoteEndPoint}");
        tcpClient.Dispose();
    }).Start();
}