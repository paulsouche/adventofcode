namespace Namespace;

public class StepMap
{
    public readonly string From;
    public readonly string To;
    public readonly List<Range> Ranges;

    public StepMap(string input)
    {
        var inputList = input.Split("\n");
        var fromTo = inputList.First().Replace(" map:", "").Split("-to-");
        From = fromTo.First();
        To = fromTo.Last();

        Ranges = inputList.Skip(1).Select((rule) =>
        {
            var ruleList = rule.Split(' ').Select(long.Parse);
            var destinationRangeStart = ruleList.First();
            var sourceRangeStart = ruleList.Skip(1).First();
            var length = ruleList.Last();
            return new Range(sourceRangeStart, destinationRangeStart, length);
        }).ToList();
    }
}
