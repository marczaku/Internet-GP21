namespace TcpExample;

public class TicTacToeGrid
{
    public CellState[,] grid = new CellState[3, 3];
    
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

    public override string ToString()
    {
        return string.Join("\n------\n", GetRows().Select(PrintRow));
    }
}