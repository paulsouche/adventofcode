namespace Namespace;

public class Hike
{
    public static string RemoveSlopes(string input) => string.Join("", input.Select(c => ">v<^".Contains(c) ? '.' : c));
    private static readonly (int x, int y) Up = (0, -1);
    private static readonly (int x, int y) Down = (0, 1);
    private static readonly (int x, int y) Left = (-1, 0);
    private static readonly (int x, int y) Right = (1, 0);
    private static readonly List<(int x, int y)> Directions = new() {
        Up,
        Down,
        Left,
        Right
    };
    private static readonly Dictionary<char, List<(int x, int y)>> Exits = new()
    {
        {'<', new () {Left}},
        {'>', new () {Right}},
        {'^', new () {Up}},
        {'v', new () {Down}},
        {'.', Directions},
        {'#', new ()},
    };
    private readonly Dictionary<(int x, int y), char> Map;

    public Hike(string input)
    {
        Map = input.Split('\n').SelectMany((l, y) => l.Select((c, x) => new KeyValuePair<(int x, int y), char>((x, y), c))).ToDictionary(c => c.Key, c => c.Value);
    }

    private bool IsFree((int x, int y) pos) => Map.ContainsKey(pos) && Map[pos] != '#';

    private bool IsRoad((int x, int y) pos) => IsFree(pos) && Directions.Count(d => IsFree((pos.x + d.x, pos.y + d.y))) == 2;

    private int Distance((int x, int y) posA, (int x, int y) posB)
    {
        var q = new Queue<((int x, int y), int)>();
        q.Enqueue((posA, 0));

        var visited = new HashSet<(int x, int y)> { posA };
        while (q.Any())
        {
            var (pos, dist) = q.Dequeue();
            foreach (var (x, y) in Exits[Map[pos]])
            {
                var posT = (pos.x + x, pos.y + y);
                if (posT == posB)
                {
                    return dist + 1;
                }
                else if (IsRoad(posT) && !visited.Contains(posT))
                {
                    visited.Add(posT);
                    q.Enqueue((posT, dist + 1));
                }
            }
        }
        return -1;
    }

    private (long[], Edge[]) Graph
    {
        get
        {
            var nodePos = (
                from pos in Map.Keys
                orderby pos.y, pos.x
                where IsFree(pos) && !IsRoad(pos)
                select pos
            ).ToArray();

            var nodes = Enumerable.Range(0, nodePos.Length).Select(i => 1L << i).ToArray();

            var edges = (
                from i in Enumerable.Range(0, nodePos.Length)
                from j in Enumerable.Range(0, nodePos.Length)
                where i != j
                let distance = Distance(nodePos[i], nodePos[j])
                where distance > 0
                select new Edge(nodes[i], nodes[j], distance)
            ).ToArray();

            return (nodes, edges);
        }
    }

    public int LongestPath
    {
        get
        {
            var (nodes, edges) = Graph;
            var (start, goal) = (nodes.First(), nodes.Last());

            var cache = new Dictionary<(long, long), int>();
            int FindLongestPath(long node, long visited)
            {
                if (node == goal)
                {
                    return 0;
                }
                else if ((visited & node) != 0)
                {
                    return int.MinValue;
                }
                var key = (node, visited);
                if (!cache.ContainsKey(key))
                {
                    cache[key] = edges
                        .Where(e => e.Start == node)
                        .Select(e => e.Distance + FindLongestPath(e.End, visited | node))
                        .Max();
                }
                return cache[key];
            }
            return FindLongestPath(start, 0);
        }
    }
}
