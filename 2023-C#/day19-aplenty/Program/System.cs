namespace Namespace;

public class System
{
    private static long ValidCombos(Dictionary<string, Range> ranges) => ranges.Aggregate(1L, (acc, range) => acc *= range.Value.End.Value - range.Value.Start.Value + 1);
    private readonly Dictionary<string, Instruction> Instructions;
    private readonly IEnumerable<Input> Inputs;

    public System(string input)
    {
        var rawInput = input.Split("\n\n");
        Instructions = rawInput.First().Split('\n').Select(l => new Instruction(l)).ToDictionary(i => i.Name, i => i);
        Inputs = rawInput.Last().Split('\n').Select(l => new Input(l));
    }

    public int AcceptedRatingNumbers
    {
        get => Inputs.Where(i => i.IsAccepted(Instructions)).Select(i => i.RatingNumber).Sum();
    }

    public long DistinctRatingCombinations
    {
        get => GetRangeLengths(
            new()
            {
                {"x", new(1, 4000) },
                {"m", new(1, 4000) },
                {"a", new(1, 4000) },
                {"s", new(1, 4000) }
            },
            Instructions["in"]);
    }

    private long GetRangeLengths(Dictionary<string, Range> ranges, Instruction instruction)
    {
        long validCombos = 0;
        foreach (var check in instruction.Checks)
        {
            Dictionary<string, Range> newRanges = ranges.Keys.ToDictionary(k => k, k => new Range(ranges[k].Start.Value, ranges[k].End.Value));
            var checkCategory = check.Category;
            var checkOperator = check.Operator;
            var checkValue = check.Value;
            var checkReturn = check.Return;

            if (checkOperator == '>')
            {
                if (ranges[checkCategory].End.Value > checkValue)
                {
                    newRanges[checkCategory] = new Range(Math.Max(newRanges[checkCategory].Start.Value, checkValue + 1), newRanges[checkCategory].End.Value);
                    if (checkReturn == "A") validCombos += ValidCombos(newRanges);
                    else if (checkReturn != "R") validCombos += GetRangeLengths(newRanges, Instructions[checkReturn]);
                    ranges[checkCategory] = new Range(ranges[checkCategory].Start.Value, checkValue);
                }
            }

            if (checkOperator == '<')
            {
                if (ranges[checkCategory].Start.Value < checkValue)
                {
                    newRanges[checkCategory] = new Range(newRanges[checkCategory].Start.Value, Math.Min(newRanges[checkCategory].End.Value, checkValue - 1));
                    if (checkReturn == "A") validCombos += ValidCombos(newRanges);
                    else if (checkReturn != "R") validCombos += GetRangeLengths(newRanges, Instructions[checkReturn]);
                    ranges[checkCategory] = new Range(checkValue, ranges[checkCategory].End.Value);
                }
            }
        }

        var fallbackReturn = instruction.FallBackReturn;
        if (fallbackReturn == "A") validCombos += ValidCombos(ranges);
        else if (fallbackReturn != "R") validCombos += GetRangeLengths(ranges, Instructions[fallbackReturn]);

        return validCombos;
    }
}
