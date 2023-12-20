namespace Namespace;

using System.Text.RegularExpressions;

public record Pulse(string SourceModule, string TargetModule, bool Value);

public record Module(string[] Inputs, Func<Pulse, IEnumerable<Pulse>> Handler);

partial class Machine
{
    [GeneratedRegex("\\w+")]
    private static partial Regex WordsRegex();

    private static Module Conjunction(string name, string[] inputs, string[] outputs)
    {
        // initially assign low value for each input:
        var state = inputs.ToDictionary(input => input, _ => false);

        return new Module(inputs, (signal) =>
        {
            state[signal.SourceModule] = signal.Value;
            var value = !state.Values.All(b => b);
            return outputs.Select(o => new Pulse(name, o, value));
        });
    }

    private static Module FlipFlop(string name, string[] inputs, string[] outputs)
    {
        var state = false;

        return new Module(inputs, (signal) =>
        {
            if (!signal.Value)
            {
                state = !state;
                return outputs.Select(o => new Pulse(name, o, state));
            }
            return Array.Empty<Pulse>();
        });
    }

    private static Module Broadcaster(string name, string[] inputs, string[] outputs)
    {
        return new Module(inputs, (s) => outputs.Select(o => new Pulse(name, o, s.Value)));
    }

    private readonly string Input;

    public Machine(string input)
    {
        Input = input;
    }

    private Dictionary<string, Module> ParseModules(string input)
    {
        input += "\nrx ->";

        var descriptions =
            from line in input.Split('\n')
            let words = WordsRegex().Matches(line).Select(m => m.Value).ToArray()
            select (Kind: line[0], Name: words.First(), Outputs: words[1..]);

        string[] inputs(string name) => descriptions.Where(d => d.Outputs.Contains(name)).Select(d => d.Name).ToArray();

        return descriptions.ToDictionary(
            d => d.Name,
            d => d.Kind switch
            {
                '&' => Conjunction(d.Name, inputs(d.Name), d.Outputs),
                '%' => FlipFlop(d.Name, inputs(d.Name), d.Outputs),
                _ => Broadcaster(d.Name, inputs(d.Name), d.Outputs)
            }
        );
    }

    private IEnumerable<Pulse> Signals(Dictionary<string, Module> gates)
    {
        var q = new Queue<Pulse>();
        q.Enqueue(new Pulse("button", "broadcaster", false));

        while (q.TryDequeue(out var signal))
        {
            yield return signal;

            if (!gates.ContainsKey(signal.TargetModule))
            {
                continue;
            }

            var handler = gates[signal.TargetModule];
            foreach (var signalT in handler.Handler(signal))
            {
                q.Enqueue(signalT);
            }
        }
    }

    private int Cycle(string input, string output)
    {
        var modules = ParseModules(input);
        for (var i = 1; ; i++) if (Signals(modules).Any(s => s.SourceModule == output && s.Value)) return i;
    }

    public long PulseProduct
    {
        get
        {
            var modules = ParseModules(Input);
            var values = Enumerable.Range(0, 1000).SelectMany(_ => Signals(modules).Select(s => s.Value)).ToArray();
            return values.Count(v => v) * values.Count(v => !v);
        }
    }

    public long IterationsNeeded
    {
        get
        {
            var modules = ParseModules(Input);
            var nand = modules["rx"].Inputs.Single();
            var branches = modules[nand].Inputs;
            return branches.Aggregate(1L, (m, branch) => m * Cycle(Input, branch));
        }
    }
}

public partial class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public long Solve()
    {
        return new Machine(Input).PulseProduct;
    }
}
