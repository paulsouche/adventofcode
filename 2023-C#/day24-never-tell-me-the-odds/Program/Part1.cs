namespace Namespace;

public record HailStone((decimal x, decimal y, decimal z) Pos, (decimal x, decimal y, decimal z) Vel)
{
    private static (decimal x, decimal y)? MeetPointXY(HailStone h1, HailStone h2)
    {
        var a = h1.Vel.y;
        var b = -h1.Vel.x;
        var c = h2.Vel.y;
        var d = -h2.Vel.x;
        var det = a * d - b * c;

        if (det == 0) return null;

        (decimal x, decimal y) = (
            h1.Vel.y * h1.Pos.x - h1.Vel.x * h1.Pos.y,
            h2.Vel.y * h2.Pos.x - h2.Vel.x * h2.Pos.y
        );

        return (d / det * x + -b / det * y, -c / det * x + a / det * y);
    }
    private static bool IsPast(HailStone h, (decimal x, decimal y) meetPoint)
    {
        if (h.Vel.x == 0) return true;
        return (meetPoint.x - h.Pos.x) / h.Vel.x < 0;
    }
    public bool IntersectXY(HailStone h, (long min, long max) boundaries)
    {
        var meetPoint = MeetPointXY(this, h);
        if (!meetPoint.HasValue) return false;
        if (
            meetPoint.Value.x < boundaries.min ||
            meetPoint.Value.x > boundaries.max ||
            meetPoint.Value.y < boundaries.min ||
            meetPoint.Value.y > boundaries.max
        ) return false;
        if (IsPast(this, meetPoint.Value) || IsPast(h, meetPoint.Value)) return false;
        return true;
    }
}

public class Space
{
    private readonly List<HailStone> HailStones;
    public Space(string input) => HailStones = input.Split('\n').Select(l =>
    {
        var h = l.Split(" @ ").SelectMany(i => i.Split(", ").Select(long.Parse)).ToList();
        return new HailStone((h[0], h[1], h[2]), (h[3], h[4], h[5]));
    }).ToList();
    public int IntersectXY((long min, long max) boundaries) => HailStones.SelectMany((fst, i) => HailStones.Skip(i + 1).Where(snd => fst.IntersectXY(snd, boundaries))).Count();
}

public partial class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public int Solve((long min, long max) boundaries) => new Space(Input).IntersectXY(boundaries);
}
