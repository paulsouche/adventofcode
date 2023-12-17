namespace Namespace;

public class UltraCrucible : Crucible
{
    public UltraCrucible(int x, int y, int walkedStraight, Directions direction) : base(x, y, walkedStraight, direction)
    {
    }

    public override bool IsAtEnd((int, int) pos) => WalkedStraight >= 4 && base.IsAtEnd(pos);

    public override ICrucible Walk(Directions d)
    {
        int walkedStraight = Direction == d ? WalkedStraight + 1 : 1;
        return d switch
        {
            Directions.NORTH => new UltraCrucible(X, Y - 1, walkedStraight, d),
            Directions.WEST => new UltraCrucible(X - 1, Y, walkedStraight, d),
            Directions.SOUTH => new UltraCrucible(X, Y + 1, walkedStraight, d),
            Directions.EAST => new UltraCrucible(X + 1, Y, walkedStraight, d),
            _ => throw new Exception("Invalid direction")
        };
    }

    public override ICrucible? Neighbour(Directions cardinal, int[][] grid)
    {
        if (cardinal == Flip()) return null;
        if (cardinal != Direction && WalkedStraight < 4) return null;
        if (cardinal == Direction && WalkedStraight == 10) return null;
        ICrucible next = Walk(cardinal);
        if (next.IsOutOfGrid(grid)) return null;
        return next;
    }
}
