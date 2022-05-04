using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;

namespace TcpExample;

public class TicTacToe
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public CellState[,] grid = new CellState[3, 3];

    public TcpClient x;
    public TcpClient o;

    private bool AnyPlayerWon => GetWinner() != CellState.Empty;
    private CellState activePlayer = CellState.X;
    
    CellState GetWinner()
    {
        return CellState.Empty;
    }

    private string inputFormat = @"{""X\"":1,""Y"":1}";

    public void Start()
    {
        Console.WriteLine("Tic Tac Toe match starting!");
        while (!AnyPlayerWon)
        {
            if (activePlayer == CellState.X)
            {
                // print information to player x
                var streamWriter = new StreamWriter(x.GetStream());
                streamWriter.WriteLine("It's your turn! Pick a Cell to write to.");
                streamWriter.WriteLine(PrintGrid());
                streamWriter.Flush();
                
                // wait for player x to make a pick
                var streamReader = new StreamReader(x.GetStream()); // 0100110101010010101 // 1/1
                Console.WriteLine($"Waiting for Player {activePlayer} to make a pick. In the format: {inputFormat}");
                var pick = JsonSerializer.Deserialize<Vector2Int>(streamReader.ReadLine());
                Console.WriteLine($"Player {activePlayer} picked: {pick}");
                
                // print the pick to player o
                var otherWriter = new StreamWriter(o.GetStream());
                otherWriter.WriteLine($"Player {activePlayer} picked: {pick}");
                otherWriter.Flush();
            }
        }
    }

    IEnumerable<IEnumerable<CellState>> GetRows()
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            yield return GetRow(y);
        }
    }

    IEnumerable<CellState> GetRow(int y)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            yield return grid[x, y];
        }
    }

    string PrintCell(CellState cellState)
    {
        return cellState switch
        {
            CellState.Empty => " ",
            _ => cellState.ToString()
        };
    }

    string PrintRow(IEnumerable<CellState> row)
    {
        return string.Join("|", row.Select(PrintCell));
    }

    string PrintGrid()
    {
        return string.Join("\n------\n", GetRows().Select(PrintRow));
    }
}