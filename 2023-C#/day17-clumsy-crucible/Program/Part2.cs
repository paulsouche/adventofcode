namespace Namespace;

public partial class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public int? Solve()
    {
        return new LavaFall(Input).UltraCrucibleMinHeatLoss;
    }
}
