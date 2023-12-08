namespace Namespace;

public class Oasis
{
    private readonly List<List<int>> Report;

    private static List<int> DifferenceAtEachStep(List<int> list)
    {
        return list.Skip(1).Select((l, i) => l - list[i]).ToList();
    }

    private static int NextExtrapolatedValue(List<int> list)
    {
        var difference = DifferenceAtEachStep(list);
        var last = list.Last();
        return difference.All(d => d == 0) ? last : last + NextExtrapolatedValue(difference);
    }

    private static int PreviousExtrapolatedValue(List<int> list)
    {
        var difference = DifferenceAtEachStep(list);
        var first = list.First();
        return difference.All(d => d == 0) ? first : first - PreviousExtrapolatedValue(difference);
    }

    public Oasis(string input)
    {
        Report = input.Split('\n').Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();
    }

    public int SumOfNextExtrapolatedValues
    {
        get => Report.Select(NextExtrapolatedValue).Sum();
    }

    public int SumOfPreviousExtrapolatedValues
    {
        get => Report.Select(PreviousExtrapolatedValue).Sum();
    }
}
