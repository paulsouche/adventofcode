namespace Namespace;

public partial class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public long Solve()
    {
        return new ConditionRecord(Input).PossibleArrangmentsSum;
    }
}
