namespace Namespace;

public class LavaFall
{
    private readonly static List<Directions> Cardinals = new() {
        Directions.NORTH,
        Directions.EAST,
        Directions.SOUTH,
        Directions.WEST,
    };
    private readonly int[][] Grid;
    public LavaFall(string input)
    {
        Grid = input.Split('\n').Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    public int? CrucibleMinHeatLoss
    {
        get => Dijkstra(new Crucible(0, 0, 0, Directions.EAST));
    }

    public int? UltraCrucibleMinHeatLoss
    {
        get => Dijkstra(new UltraCrucible(0, 0, 0, Directions.EAST));
    }

    private int? Dijkstra(ICrucible start)
    {
        int targetY = Grid.Length - 1;
        int targetX = Grid[0].Length - 1;
        Dictionary<string, int> heatLosses = new();

        PriorityQueue<ICrucible, int> queue = new();
        heatLosses[start.ToHash()] = 0;
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var crucible, out int heatLoss))
        {
            if (crucible.IsAtEnd((targetX, targetY))) return heatLoss;
            foreach (Directions cardinal in Cardinals)
            {
                ICrucible? next = crucible.Neighbour(cardinal, Grid);
                if (next == null) continue;

                int cost = heatLosses[crucible.ToHash()] + next.HeatLoss(Grid);

                if (cost < heatLosses.GetValueOrDefault(next.ToHash(), int.MaxValue))
                {
                    heatLosses[next.ToHash()] = cost;
                    queue.Enqueue(next, cost + next.ManhattanDistance((targetX, targetY)));
                }
            }
        }

        return null;
    }
}
