namespace Namespace;

public class Crucible : ICrucible
{
    public readonly int X;
    public readonly int Y;
    public readonly int WalkedStraight;
    public readonly Directions Direction;
    public Crucible(int x, int y, int walkedStraight, Directions direction)
    {
        X = x;
        Y = y;
        WalkedStraight = walkedStraight;
        Direction = direction;
    }

    public string ToHash() => $"{X},{Y},{Direction},{WalkedStraight}";

    public bool IsOutOfGrid(int[][] grid) => Y < 0 || Y >= grid.Length || X < 0 || X >= grid[0].Length;

    public virtual bool IsAtEnd((int, int) pos)
    {
        (int targetX, int targetY) = pos;
        return X == targetX && Y == targetY;
    }

    public int HeatLoss(int[][] grid) => grid[Y][X];

    public int ManhattanDistance((int, int) pos)
    {
        (int x, int y) = pos;
        return Math.Abs(X - x) + Math.Abs(Y - y);
    }

    public Directions Flip()
    {
        return Direction switch
        {
            Directions.NORTH => Directions.SOUTH,
            Directions.WEST => Directions.EAST,
            Directions.SOUTH => Directions.NORTH,
            Directions.EAST => Directions.WEST,
            _ => throw new Exception("Invalid direction")
        };
    }

    public virtual ICrucible Walk(Directions d)
    {
        int walkedStraight = Direction == d ? WalkedStraight + 1 : 1;
        return d switch
        {
            Directions.NORTH => new Crucible(X, Y - 1, walkedStraight, d),
            Directions.WEST => new Crucible(X - 1, Y, walkedStraight, d),
            Directions.SOUTH => new Crucible(X, Y + 1, walkedStraight, d),
            Directions.EAST => new Crucible(X + 1, Y, walkedStraight, d),
            _ => throw new Exception("Invalid direction")
        };
    }

    public virtual ICrucible? Neighbour(Directions cardinal, int[][] grid)
    {
        if (cardinal == Flip()) return null;
        if (cardinal == Direction && WalkedStraight == 3) return null;
        ICrucible next = Walk(cardinal);
        if (next.IsOutOfGrid(grid)) return null;
        return next;
    }
}
