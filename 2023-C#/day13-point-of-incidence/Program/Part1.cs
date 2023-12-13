namespace Namespace;

public class Pattern
{
    private static int? SummarizeList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var remaining = list.Skip(i + 1);
            if (!remaining.Any() || remaining.Where((l, j) => j <= i && l != list[i - j]).Any())
            {
                continue;
            }
            return i + 1;
        }
        return null;
    }

    private readonly string[] RawInput;

    public Pattern(string input)
    {
        RawInput = input.Split('\n');
    }

    private IEnumerable<string> HorizontalLines
    {
        get => RawInput;
    }

    private IEnumerable<string> VerticalLines
    {
        get => RawInput.First().ToCharArray().Select((c, i) => string.Join("", RawInput.Select(l => l[i])));
    }

    public int Summary
    {
        get
        {
            var horizontalSummary = SummarizeList(HorizontalLines.ToList());
            if (horizontalSummary.HasValue)
            {
                return 100 * horizontalSummary.Value;
            }

            var verticalSummary = SummarizeList(VerticalLines.ToList());
            if (verticalSummary.HasValue)
            {
                return verticalSummary.Value;
            }

            throw new Exception("Should not happen");
        }
    }
}

public class Notes
{
    private readonly IEnumerable<Pattern> Patterns;

    public Notes(string input)
    {
        Patterns = input.Split("\n\n").Select(p => new Pattern(p));
    }

    public int Summary
    {
        get => Patterns.Select(p => p.Summary).Sum();
    }
}

public partial class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public long Solve()
    {
        return new Notes(Input).Summary;
    }
}
