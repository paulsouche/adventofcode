namespace Namespace;

public class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public double Solve() => new ScratchCards(Input).Points;
}
