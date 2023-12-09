namespace Namespace;

public class Maze
{
    private static Directions? NextDirection(Cell cell, Position pos)
    {
        switch (cell.Value)
        {
            case '-':
                if (pos.Direction == Directions.West || pos.Direction == Directions.East)
                {
                    return pos.Direction;
                }
                return null;
            case '|':
                if (pos.Direction == Directions.North || pos.Direction == Directions.South)
                {
                    return pos.Direction;
                }
                return null;
            case 'F':
                if (pos.Direction == Directions.North || pos.Direction == Directions.West)
                {
                    return pos.Direction == Directions.North ? Directions.East : Directions.South;
                }
                return null;
            case '7':
                if (pos.Direction == Directions.North || pos.Direction == Directions.East)
                {
                    return pos.Direction == Directions.North ? Directions.West : Directions.South;
                }
                return null;
            case 'J':
                if (pos.Direction == Directions.South || pos.Direction == Directions.East)
                {
                    return pos.Direction == Directions.South ? Directions.West : Directions.North;
                }
                return null;
            case 'L':
                if (pos.Direction == Directions.South || pos.Direction == Directions.West)
                {
                    return pos.Direction == Directions.South ? Directions.East : Directions.North;
                }
                return null;
            default:
                return null;
        }
    }

    private readonly static Dictionary<Directions, (int, int)> CoordinatesChange;
    static Maze()
    {
        CoordinatesChange = new()
        {
            {Directions.North, (0,-1)},
            {Directions.East, (1,0)},
            {Directions.South, (0,1)},
            {Directions.West, (-1,0)},
        };
    }
    private List<List<Cell>> Cells;
    public Maze(string input)
    {
        Cells = input.Split('\n').Select(l => l.ToCharArray().Select(c => new Cell(c)).ToList()).ToList();
        (int, int)? start = null;
        for (int y = 0; y < Cells.Count; y++)
        {
            for (int x = 0; x < Cells[y].Count; x++)
            {
                if (Cells[y][x].IsStart)
                {
                    start = (x, y);
                }
            }
            if (start.HasValue) break;
        }

        if (!start.HasValue)
        {
            throw new Exception("Could not find start");
        }

        List<Position> runs = new()
        {
            new Position(start.Value, Directions.North, 0),
            new Position(start.Value, Directions.East, 0),
            new Position(start.Value, Directions.South, 0),
            new Position(start.Value, Directions.West, 0),
        };
        foreach (Position run in runs)
        {
            Position? pos = run;
            while (pos != null) pos = Move(pos);
        }

        Queue<(int, int)> queue = new();
        for (int x = 0; x < Cells[0].Count; x++)
        {
            queue.Enqueue((x, 0));
            queue.Enqueue((x, Cells.Count - 1));
        }
        for (int y = 1; y < Cells.Count - 1; y++)
        {
            queue.Enqueue((0, y));
            queue.Enqueue((Cells.First().Count - 1, y));
        }

        while (queue.Any())
        {
            var (x, y) = queue.Dequeue();
            if (Cells[y][x].IsVisited || Cells[y][x].IsOutside)
            {
                continue;
            }

            Cells[y][x].IsOutside = true;

            List<(int, int)> neighbours = new()
            {
                (x - 1, y),
                (x + 1, y),
                (x, y - 1),
                (x, y + 1),
            };
            foreach ((int, int) neighbour in neighbours)
            {
                var (newX, newY) = neighbour;
                if (newX < 0 || newY < 0 || newY >= Cells.Count || newX >= Cells.First().Count)
                {
                    continue;
                }
                var cell = Cells[newY][newX];
                if (!cell.IsVisited && !cell.IsOutside)
                {
                    queue.Enqueue((newX, newY));
                }
            }
        }


    }

    private Position? Move(Position pos)
    {
        var change = CoordinatesChange[pos.Direction];
        var (x, y) = pos.Coordinates;
        var (newX, newY) = (x + change.Item1, y + change.Item2);
        if (newX < 0 || newY < 0)
        {
            return null;
        }

        var end = Cells[newY][newX];
        var newDistance = pos.Distance + 1;

        if (end.Value == 'S' || end.DistanceFromStart <= newDistance)
        {
            return null;
        }

        var newDirection = NextDirection(end, pos);

        if (!newDirection.HasValue)
        {
            return null;
        }

        end.DistanceFromStart = newDistance;
        end.IsVisited = true;
        return new Position((newX, newY), newDirection.Value, newDistance);
    }

    public int FarthestDistanceFromStart
    {
        get => Cells.Select(l => l.Where(c => c.IsVisited).Select(c => c.DistanceFromStart).DefaultIfEmpty(0).Max()).Max();
    }

    public int TilesInsideTheLoop
    {
        get => Cells.Select(l => l.Where(c => !c.IsVisited && !c.IsOutside).Count()).Sum();
    }
}
