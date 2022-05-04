using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;

namespace TcpExample;

public class TicTacToe
{
    private TicTacToeGrid grid = new();

    public TcpClient x;
    public TcpClient o;

    private bool GameEnds => AnyPlayerWon || GetGridIsFull();
    private bool AnyPlayerWon => GetWinner() != CellState.Empty;
    private CellState activePlayer = CellState.X;
    
    CellState GetWinner()
    {
        // TODO: actually check for winner (all rows, columns, diagonals)
        return CellState.Empty;
    }

    bool GetGridIsFull()
    {
        // TODO: actually check, whether the grid is full
        return false;
    }

    private string inputFormat = @"{""X"":1,""Y"":1}";

    public void Start()
    {
        Console.WriteLine("Tic Tac Toe match starting!");
        while (!GameEnds)
        {
            TcpClient activeClient;
            TcpClient waitingClient;
            if (activePlayer == CellState.X)
            {
                activeClient = x;
                waitingClient = o;
            }
            else
            {
                activeClient = o;
                waitingClient = x;
            }

            // print information to active player
            var streamWriter = new StreamWriter(activeClient.GetStream());
            streamWriter.WriteLine("It's your turn! Pick a Cell to write to.");
            streamWriter.WriteLine(grid);
            streamWriter.Flush();
            
            // wait for active player to make a pick
            var streamReader = new StreamReader(activeClient.GetStream()); // 0100110101010010101 // 1/1
            Console.WriteLine($"Waiting for Player {activePlayer} to make a pick. In the format: {inputFormat}");
            var pick = JsonSerializer.Deserialize<Vector2Int>(streamReader.ReadLine());
            Console.WriteLine($"Player {activePlayer} picked: {pick}");
            
            // TODO validate the input (we cannot override a cell that's already being used
            // TODO change the grid at the picked position
            //grid.grid[pick.X...] = ...
            
            // print the pick to waiting player
            var otherWriter = new StreamWriter(waitingClient.GetStream());
            otherWriter.WriteLine($"Player {activePlayer} picked: {pick}");
            otherWriter.Flush();
            
            // TODO switch activePlayer (if it was a valid pick)
        }
        
        // TODO: Announce Winner (or Draw)
    }
    
}