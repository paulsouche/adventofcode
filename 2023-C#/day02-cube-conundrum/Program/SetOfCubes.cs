namespace Namespace;
using System.Text.RegularExpressions;

public class SetOfCubes
{
    private static readonly string RedSetRegex = @"(\d+)\sred";
    private static readonly string GreenSetRegex = @"(\d+)\sgreen";
    private static readonly string BlueSetRegex = @"(\d+)\sblue";

    public readonly int Red;
    public readonly int Green;
    public readonly int Blue;

    public SetOfCubes(string input)
    {
        var match = Regex.Matches(input, RedSetRegex);
        Red = match.Count > 0 ? int.Parse(match[0].Groups[1].Value) : 0;
        match = Regex.Matches(input, GreenSetRegex);
        Green = match.Count > 0 ? int.Parse(match[0].Groups[1].Value) : 0;
        match = Regex.Matches(input, BlueSetRegex);
        Blue = match.Count > 0 ? int.Parse(match[0].Groups[1].Value) : 0;
    }

    public SetOfCubes(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public int Power
    {
        get => Red * Green * Blue;
    }
}
