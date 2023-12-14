namespace Namespace;

public class Rock
{
    private readonly int Height;
    public int X;
    public int Y;

    public Rock(int x, int y, int height)
    {
        Height = height;
        X = x;
        Y = y;
    }

    public void Tilt(Directions direction)
    {
        switch (direction)
        {
            case Directions.NORTH:
                Y--;
                break;
            case Directions.WEST:
                X--;
                break;
            case Directions.SOUTH:
                Y++;
                break;
            case Directions.EAST:
                Y--;
                break;
        }
    }

    public int Weight
    {
        get => Height - Y;
    }
}
