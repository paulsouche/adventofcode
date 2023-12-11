namespace Namespace;

public partial class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public long Solve(long times = 1)
    {
        return new Universe(Input, times).DistanceBetweenGalaxiesSum;
    }
}
