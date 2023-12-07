namespace Namespace;

public class Node
{
    public readonly string Value;
    public readonly string Left;
    public readonly string Right;
    public Node(string input)
    {
        var rawInput = input.Split(" = ");
        Value = rawInput.First();
        var rawDirections = rawInput.Last().Split(", ");
        Left = rawDirections.First().Replace("(", string.Empty);
        Right = rawDirections.Last().Replace(")", string.Empty);
    }
}
