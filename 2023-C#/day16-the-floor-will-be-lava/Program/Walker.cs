namespace Namespace;

public class Walker
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private Directions Direction;
    public Walker(int x, int y, Directions direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

    public string Coordinates
    {
        get => $"{X},{Y}";
    }

    public string Hash
    {
        get => $"{Coordinates},{Direction}";
    }

    public Walker Walk()
    {
        switch (Direction)
        {
            case Directions.NORTH:
                Y--;
                break;
            case Directions.EAST:
                X++;
                break;
            case Directions.SOUTH:
                Y++;
                break;
            case Directions.WEST:
                X--;
                break;
        }
        return this;
    }

    public Walker Turn(char[][] grid)
    {
        var tile = grid[Y][X];
        switch (Direction)
        {
            case Directions.NORTH:
                if (tile == '\\') Direction = Directions.WEST;
                if (tile == '/') Direction = Directions.EAST;
                break;
            case Directions.EAST:
                if (tile == '\\') Direction = Directions.SOUTH;
                if (tile == '/') Direction = Directions.NORTH;
                break;
            case Directions.SOUTH:
                if (tile == '\\') Direction = Directions.EAST;
                if (tile == '/') Direction = Directions.WEST;
                break;
            case Directions.WEST:
                if (tile == '\\') Direction = Directions.NORTH;
                if (tile == '/') Direction = Directions.SOUTH;
                break;
        }
        return this;
    }

    public List<Walker> Split(char[][] grid)
    {
        var tile = grid[Y][X];
        switch (Direction)
        {
            case Directions.NORTH:
            case Directions.SOUTH:
                if (tile == '-')
                {
                    return new(){
                        new Walker(X, Y, Directions.EAST),
                        new Walker(X, Y, Directions.WEST),
                    };
                };
                break;
            case Directions.EAST:
            case Directions.WEST:
                if (tile == '|')
                {
                    return new(){
                        new Walker(X, Y, Directions.NORTH),
                        new Walker(X, Y, Directions.SOUTH),
                    };
                }
                break;
        }
        return new() { this };
    }

    public bool IsOutOfGrid(char[][] grid) => Y < 0 || Y >= grid.Length || X < 0 || X >= grid[0].Length;
}
