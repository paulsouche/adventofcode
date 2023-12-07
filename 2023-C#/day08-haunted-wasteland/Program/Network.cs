namespace Namespace;

public class Network
{
    private static long Ppcm(long a, long b)
    {
        long p = a * b;
        while (a != b) if (a < b) b -= a; else a -= b;
        return p / a;
    }

    private readonly char[] Instructions;
    private readonly Dictionary<string, Node> Nodes;

    public Network(string input)
    {
        var rawInput = input.Split("\n\n");
        Instructions = rawInput.First().ToCharArray();
        Nodes = rawInput.Last().Split('\n').Select(i => new Node(i)).ToDictionary(n => n.Value, n => n);
    }

    public int NumberOfStepsToZZZ
    {
        get
        {
            var pos = "AAA";
            int steps = 0;

            while (pos != "ZZZ")
            {
                foreach (char instruction in Instructions)
                {
                    pos = instruction == 'L' ? Nodes[pos].Left : Nodes[pos].Right;
                    steps++;
                }
            }

            return steps;
        }
    }

    public long GhostNumberOfStepsToZ
    {
        get
        {
            List<string> starts = Nodes.Keys.Where(k => k.EndsWith("A")).ToList();
            long totalSteps = 1;
            foreach (string start in starts)
            {
                string pos = start;
                long steps = 0;
                while (!pos.EndsWith("Z"))
                {
                    foreach (char instruction in Instructions)
                    {
                        pos = instruction == 'L' ? Nodes[pos].Left : Nodes[pos].Right;
                        steps++;
                    }
                }
                totalSteps = Ppcm(totalSteps, steps);
            }
            return totalSteps;
        }
    }
}
