namespace Namespace;

public class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public long Solve(long seedStart = 0) => new Garden(Input).GetLowestLocationNumberForSeedsRanges(seedStart);
}
