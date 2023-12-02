namespace Namespace;

public class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public int Solve() => new Engine(Input).GearsRatioSum;
}
