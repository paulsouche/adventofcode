namespace Namespace;

public class Dish
{
    private readonly Dictionary<string, Cell> Cells;

    private List<Rock> Rocks;

    public Dish(string input)
    {
        var lines = input.Split('\n');
        Cells = lines.SelectMany((l, y) => l.ToCharArray().Select((c, x) => new Cell(x, y, c == '#' ? '#' : '.'))).ToDictionary(c => $"{c.X},{c.Y}", c => c);
        Rocks = lines.SelectMany((l, y) => l.ToCharArray().Select((c, x) => c == 'O' ? new Rock(x, y, lines.Length) : null).Where(r => r != null).Select(r => r!)).ToList();
    }

    private Dish(Dictionary<string, Cell> cells, List<Rock> rocks)
    {
        Cells = cells;
        Rocks = rocks;
    }

    public Dish Tilt(Directions direction)
    {
        var cells = Cells.ToDictionary(e => e.Key, e => e.Value);
        foreach (Rock rock in Rocks)
        {
            switch (direction)
            {
                case Directions.NORTH:
                    while (cells.ContainsKey($"{rock.X},{rock.Y - 1}") && cells[$"{rock.X},{rock.Y - 1}"].V == '.') rock.Tilt(direction);
                    break;
                case Directions.WEST:
                    while (cells.ContainsKey($"{rock.X - 1},{rock.Y}") && cells[$"{rock.X - 1},{rock.Y}"].V == '.') rock.Tilt(direction);
                    break;
                case Directions.SOUTH:
                    while (cells.ContainsKey($"{rock.X},{rock.Y + 1}") && cells[$"{rock.X},{rock.Y + 1}"].V == '.') rock.Tilt(direction);
                    break;
                case Directions.EAST:
                    while (cells.ContainsKey($"{rock.X + 1},{rock.Y}") && cells[$"{rock.X + 1},{rock.Y}"].V == '.') rock.Tilt(direction);
                    break;
            }
            cells[$"{rock.X},{rock.Y}"] = new Cell(rock.X, rock.Y, '#');
        }
        return new Dish(Cells, Rocks);
    }

    public Dish Cycle() => Tilt(Directions.NORTH).Tilt(Directions.WEST).Tilt(Directions.SOUTH).Tilt(Directions.EAST);

    public int TotalWeight
    {
        get => Rocks.Select(r => r.Weight).Sum();
    }
}
