namespace Namespace;

using System.Text.RegularExpressions;

public class Game
{
    private static readonly string GameInputRegex = @"^Game\s(\d+):(.*)$";
    public readonly int Id;
    private readonly IEnumerable<SetOfCubes> SetOfCubes;

    public Game(string input)
    {
        var match = Regex.Matches(input, GameInputRegex);
        Id = int.Parse(match[0].Groups[1].Value);
        SetOfCubes = match[0].Groups[2].Value.Split(';').Select(set => new SetOfCubes(set));
    }

    public bool IsPossibleWithSet(SetOfCubes withSet) => SetOfCubes.All(s => s.Red <= withSet.Red & s.Green <= withSet.Green & s.Blue <= withSet.Blue);

    public SetOfCubes SetOfCubesForGamePossible
    {
        get
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            foreach (SetOfCubes setOfCubes in SetOfCubes)
            {
                red = int.Max(red, setOfCubes.Red);
                green = int.Max(green, setOfCubes.Green);
                blue = int.Max(blue, setOfCubes.Blue);
            }

            return new SetOfCubes(red, green, blue);
        }
    }
}
