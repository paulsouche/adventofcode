namespace Namespace;

public class SquareGrid
{
    private readonly char[][] Grid;
    public SquareGrid(string input)
    {
        Grid = input.Split('\n').Select(l => l.ToCharArray()).ToArray();
    }

    public int EnergizedTiles
    {
        get => Walk(new Walker(-1, 0, Directions.EAST));
    }

    public int MaxEnergizedTiles
    {
        get
        {
            List<int> possibleEnergizedTiles = new();

            for (int y = 0; y < Grid.Length; y++)
            {
                possibleEnergizedTiles.Add(Walk(new Walker(-1, y, Directions.EAST)));
                possibleEnergizedTiles.Add(Walk(new Walker(Grid[0].Length, y, Directions.WEST)));
            }

            for (int x = 0; x < Grid[0].Length; x++)
            {
                possibleEnergizedTiles.Add(Walk(new Walker(x, -1, Directions.SOUTH)));
                possibleEnergizedTiles.Add(Walk(new Walker(x, Grid.Length, Directions.NORTH)));
            }

            return possibleEnergizedTiles.Max();
        }
    }

    private int Walk(Walker startWalker)
    {
        var queue = new Queue<Walker>();
        var visited = new HashSet<string>();
        var wasTheSameBefore = new HashSet<string>();
        queue.Enqueue(startWalker);
        while (queue.Any())
        {
            var walker = queue.Dequeue();
            if (walker.Walk().IsOutOfGrid(Grid))
            {
                continue;
            }
            var newWalkers = walker.Turn(Grid).Split(Grid);
            foreach (Walker newWalker in newWalkers)
            {
                if (wasTheSameBefore.Contains(newWalker.Hash))
                {
                    continue;
                }
                visited.Add(newWalker.Coordinates);
                wasTheSameBefore.Add(newWalker.Hash);
                queue.Enqueue(newWalker);
            }
            // Print(visited);
        }
        return visited.Count;
    }

    private void Print(HashSet<string> visited)
    {
        for (int y = 0; y < Grid.Length; y++)
        {
            Console.WriteLine(string.Join("", Grid[y].Select((c, x) => visited.Contains($"{x},{y}") ? '#' : c)));
        }
        Console.WriteLine("");
    }
}
