namespace Namespace;

public class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public int Solve(SetOfCubes setOfCubes) =>
        Input.Split('\n')
          .Select(s => new Game(s))
          .Where(g => g.IsPossibleWithSet(setOfCubes))
          .Select(g => g.Id)
          .Sum();
}
