
namespace Namespace;

public class Tower
{
    private static bool IntersectsXY(Brick brickA, Brick brickB) =>
        Intersects(brickA.X, brickB.X) && Intersects(brickA.Y, brickB.Y);

    private static bool Intersects(Range r1, Range r2) => r1.Start.Value <= r2.End.Value && r2.Start.Value <= r1.End.Value;

    private readonly List<Brick> Bricks;

    public Tower(string input)
    {
        var bricks = input.Split('\n').Select(l => l.Split(',', '~').Select(int.Parse).ToArray()).Select(l => new Brick(
            X: new Range(l[0], l[3]),
            Y: new Range(l[1], l[4]),
            Z: new Range(l[2], l[5])
        )).ToList();

        bricks = bricks.OrderBy(brick => brick.Bottom).ToList();

        for (var i = 0; i < bricks.Count; i++)
        {
            var newBottom = 1;
            for (var j = 0; j < i; j++)
            {
                if (IntersectsXY(bricks[i], bricks[j]))
                {
                    newBottom = Math.Max(newBottom, bricks[j].Top + 1);
                }
            }
            var fall = bricks[i].Bottom - newBottom;
            bricks[i] = bricks[i] with
            {
                Z = new Range(bricks[i].Bottom - fall, bricks[i].Top - fall)
            };
        }
        Bricks = bricks;
    }

    private Supports Supports
    {
        get
        {
            var bricksAbove = Bricks.ToDictionary(b => b, _ => new HashSet<Brick>());
            var bricksBelow = Bricks.ToDictionary(b => b, _ => new HashSet<Brick>());
            for (var i = 0; i < Bricks.Count; i++)
            {
                for (var j = i + 1; j < Bricks.Count; j++)
                {
                    var zNeighbours = Bricks[j].Bottom == 1 + Bricks[i].Top;
                    if (zNeighbours && IntersectsXY(Bricks[i], Bricks[j]))
                    {
                        bricksBelow[Bricks[j]].Add(Bricks[i]);
                        bricksAbove[Bricks[i]].Add(Bricks[j]);
                    }
                }
            }
            return new Supports(bricksAbove, bricksBelow);
        }
    }

    private IEnumerable<int> Disintegrate()
    {
        var bricks = Bricks;
        var supports = Supports;

        foreach (var desintegratedBrick in bricks)
        {
            var q = new Queue<Brick>();
            q.Enqueue(desintegratedBrick);

            var falling = new HashSet<Brick>();
            while (q.TryDequeue(out var brick))
            {
                falling.Add(brick);

                var blocksStartFalling = supports.BricksAbove[brick].Where(b => supports.BricksBelow[b].IsSubsetOf(falling));

                foreach (var blockT in blocksStartFalling)
                {
                    q.Enqueue(blockT);
                }
            }
            yield return falling.Count - 1;
        }
    }

    public int Disintegrable
    {
        get => Disintegrate().Count(x => x == 0);
    }

    public int SumOfFallingBricks
    {
        get => Disintegrate().Sum();
    }
}
