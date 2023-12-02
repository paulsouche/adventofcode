namespace Namespace;

public class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public int Solve() =>
        Input.Split('\n')
          .Select(s => new Game(s).SetOfCubesForGamePossible.Power)
          .Sum();
}
