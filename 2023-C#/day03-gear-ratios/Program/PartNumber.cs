namespace Namespace;

public class PartNumber
{
    private readonly HashSet<string> SurroundingCoordinates;
    public readonly int Value;

    public PartNumber(string value, int x, int y)
    {
        Value = int.Parse(value);
        SurroundingCoordinates = new()
        {
            $"{x - value.Length - 1},{y}",
            $"{x},{y}"
        };
        for (int i = x - value.Length - 1; i <= x; i++)
        {
            SurroundingCoordinates.Add($"{i},{y - 1}");
            SurroundingCoordinates.Add($"{i},{y + 1}");
        }
    }

    public bool IsAdjacentToSymbol(Dictionary<string, Cell> cells) => SurroundingCoordinates.Any(sc => cells.ContainsKey(sc) && cells[sc].IsSymbol);

    public bool IsAdjacentToGear(string gearCoordinates) => SurroundingCoordinates.Contains(gearCoordinates);
}