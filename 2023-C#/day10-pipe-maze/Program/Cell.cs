namespace Namespace;

public class Cell
{
    public readonly char Value;
    public int DistanceFromStart { get; set; }
    public bool IsVisited { get; set; }
    public bool IsOutside { get; set; }

    public Cell(char value)
    {
        Value = value;
        DistanceFromStart = int.MaxValue;
        IsVisited = false;
    }

    public bool IsStart
    {
        get => Value == 'S';
    }
}
