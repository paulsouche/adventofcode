namespace Namespace;

public class Farm
{

    private readonly Dictionary<(int x, int y), bool> Map;
    private (int x, int y) Start;
    private readonly static List<(int x, int y)> Neighbours = new()
    {
        ( 0,-1),
        ( 1, 0),
        ( 0, 1),
        (-1, 0),
    };

    public Farm(string input)
    {
        Start = (0, 0);
        Map = input.Split('\n').SelectMany((l, y) => l.ToCharArray().Select((c, x) =>
        {
            if (c == 'S') Start = (x, y);
            return ((x, y), c);
        })).ToDictionary(coords => coords.Item1, coords => coords.c switch
            {
                '#' => false,
                _ => true,
            }
        );
    }

    public HashSet<(int x, int y)> Walk(HashSet<(int x, int y)> reached)
    {
        return reached.SelectMany(r => Neighbours.Select(n => (n.x + r.x, n.y + r.y)).Where(n => Map.GetValueOrDefault(n, false))).ToHashSet();
    }

    public int GetReachablePos(int rounds)
    {
        HashSet<(int x, int y)> reached = new() {
            Start,
        };
        foreach (int round in Enumerable.Range(0, rounds))
        {
            // Print(reached);
            reached = Walk(reached);
        }
        return reached.Count;
    }

    // private void Print(HashSet<(int x, int y)> reached) {
    //     var maxX = Map.Keys.Select(pos => pos.x).Max();
    //     var maxY = Map.Keys.Select(pos => pos.y).Max();
    //     foreach (int y in Enumerable.Range(0, maxY)) {
    //         Console.WriteLine(string.Join("", Enumerable.Range(0, maxX).Select(x => reached.Contains((x, y)) ? 'O' : Map[(x,y)] ? '.' : '#')));
    //     }
    //     Console.WriteLine("");
    // }
}

public partial class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public int Solve(int rounds)
    {
        return new Farm(Input).GetReachablePos(rounds);
    }
}
