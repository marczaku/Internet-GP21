namespace TcpExample;

public struct Vector2Int
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"{nameof(Vector2Int)}: {nameof(X)}: {X}, {nameof(Y)}: {Y}";
    }
}