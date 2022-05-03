using System.Net;
using System.Net.Sockets;

var tcpClient = new TcpClient();
Console.WriteLine("Connecting to server...");
tcpClient.Connect(new IPEndPoint(IPAddress.Loopback, 12244));
Console.WriteLine("Connected.");

var stream = tcpClient.GetStream();
var streamReader = new StreamReader(stream);
var streamWriter = new StreamWriter(stream);
streamWriter.AutoFlush = true;

Console.WriteLine(streamReader.ReadLine());
streamWriter.WriteLine(Console.ReadLine());
Console.WriteLine(streamReader.ReadLine());