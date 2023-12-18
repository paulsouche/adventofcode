namespace Namespace;

public class Instruction
{
    private static readonly Dictionary<string, (long, long)> DXY = new() {
        { "U", (  0,  1) },
        { "R", (  1,  0) },
        { "D", (  0, -1) },
        { "L", ( -1,  0) },
    };

    private static readonly Dictionary<char, string> DigDirection = new() {
        { '0', "R" },
        { '1', "D" },
        { '2', "L" },
        { '3', "U" },
    };
    public readonly (long, long) Dxy;
    public readonly long Distance;
    public Instruction(string input, bool parseColors = false)
    {
        string[] rawInput = input.Split(' ');
        if (parseColors)
        {
            var color = rawInput.Last().Substring(2, 6);
            Dxy = DXY[DigDirection[color[5]]];
            Distance = Convert.ToInt64(color[..5], 16);
        }
        else
        {
            Dxy = DXY[rawInput[0]];
            Distance = long.Parse(rawInput[1]);
        }
    }
}
