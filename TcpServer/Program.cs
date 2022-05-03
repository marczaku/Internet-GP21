using System.Net;
using System.Net.Sockets;

// start listening to new connections on the given socket
var tcpListener = new TcpListener(IPAddress.Any, 12244);
tcpListener.Start();

Console.WriteLine("Server listening on: "+tcpListener.LocalEndpoint);

string[] options = new[]{"Rock", "Paper", "Scissors"};

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
        
        
        streamWriter.WriteLine("Welcome to Rock, Paper, Scissors! Pick 'Rock', 'Paper' or 'Scissors' or 'Quit'.");
        var random = new Random();
        int playerScore = 0;
        int aiScore = 0;
        while (true)
        {
            // read all information currently buffered on the socket's stream
            var input = streamReader.ReadLine();
            if (input == "Quit")
                break;
            var randomNumber = random.Next(0, options.Length);
            var aiOption = options[randomNumber];
            switch (input)
            {
                case "Rock":
                case "Paper":
                case "Scissors":
                    Evaluate(input, aiOption);
                    break;
                default:
                    streamWriter.WriteLine("Wrong Input. Try again.");
                    break;
            }
        }

        void Evaluate(string player, string ai)
        {
            streamWriter.Write($"You chose {player}. AI chose {ai}. - ");
            if (player == ai)
            {
                streamWriter.WriteLine("It's a Draw. Better luck next time.");
                return;
            }

            if (player == "Rock")
            {
                if (ai == "Scissors")
                    PlayerWin();
                else
                    AiWin();
            }
            else if (player == "Paper")
            {
                if (ai == "Scissors")
                    AiWin();
                else
                    PlayerWin();
            }
            else
            {
                if (ai == "Paper")
                    PlayerWin();
                else
                    AiWin();
            }
        }

        void AiWin()
        {
            aiScore++;
            streamWriter.WriteLine($"The AI wins. Player: {playerScore} - AI: {aiScore}");
        }

        void PlayerWin()
        {
            playerScore++;
            streamWriter.WriteLine($"The Player wins. Player: {playerScore} - AI: {aiScore}");
        }
        
        Console.WriteLine($"Closing the connection to {tcpClient.Client.RemoteEndPoint}");
        tcpClient.Dispose();
    }).Start();
}