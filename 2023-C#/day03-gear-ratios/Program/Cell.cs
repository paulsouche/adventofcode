namespace Namespace;

public class Cell
{
    private readonly int X;
    private readonly int Y;
    public readonly bool IsSymbol;
    public readonly bool IsNumber;
    public readonly bool IsEmpty;
    public readonly bool IsGear;
    public readonly char Value;

    public Cell(int x, int y, char value)
    {
        X = x;
        Y = y;
        Value = value;
        IsNumber = char.IsNumber(value);
        IsEmpty = value == '.';
        IsGear = value == '*';
        IsSymbol = !IsNumber & !IsEmpty;
    }

    public string Coordinates
    {
        get => $"{X},{Y}";
    }
}
