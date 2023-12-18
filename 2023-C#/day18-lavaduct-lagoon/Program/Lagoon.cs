namespace Namespace;

public class Lagoon
{
    private readonly IEnumerable<Instruction> Instructions;

    public Lagoon(string input, bool parseColors = false)
    {
        Instructions = input.Split('\n').Select(l => new Instruction(l, parseColors));
    }

    private IEnumerable<(long, long)> Vertices
    {
        get
        {
            long x = 0;
            long y = 0;
            return Instructions.Select(i =>
            {
                (var dx, var dy) = i.Dxy;
                x += dx * i.Distance;
                y += dy * i.Distance;
                return (x, y);
            });
        }
    }

    private long Shoelace
    {
        get
        {
            long sum = 0;
            var vertices = Vertices.ToList();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                (var ax, var ay) = vertices[i];
                (var bx, var by) = vertices[i + 1];
                sum += (bx - ax) * (by + ay);
            }
            return Math.Abs(sum);
        }
    }

    private long Perimeter
    {
        get => Instructions.Select(i => i.Distance).Sum();
    }

    public long LavaCubicMeters
    {
        get => (Shoelace + Perimeter) / 2 + 1;
    }
}
