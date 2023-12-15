namespace Namespace;

public class InitializationSequence
{
    private readonly string[] Steps;
    private readonly Dictionary<int, List<Lens>> Boxes;
    public InitializationSequence(string input)
    {
        Steps = input.Split(',');
        Boxes = new();
    }
    private InitializationSequence(string[] steps, Dictionary<int, List<Lens>> boxes)
    {
        Steps = steps;
        Boxes = boxes;
    }
    public InitializationSequence HashMap()
    {
        foreach (string step in Steps)
        {
            var label = step.Replace("-", "").Split('=').First();
            var correspondingBox = Hash(label);
            if (!Boxes.ContainsKey(correspondingBox))
            {
                Boxes[correspondingBox] = new();
            }
            if (step.EndsWith('-'))
            {
                Boxes[correspondingBox] = Boxes[correspondingBox].Where(l => l.Label != label).ToList();
            }
            else
            {
                var focalLength = int.Parse(step.Split('=').Last());
                var previousLens = Boxes[correspondingBox].Find(l => l.Label == label);
                if (previousLens != null)
                {
                    previousLens.ChangeFocalLength(focalLength);
                }
                else
                {
                    Boxes[correspondingBox].Add(new Lens(label, focalLength));
                }
            }

            // PrintStep(step);
        }

        return new InitializationSequence(Steps, Boxes);
    }
    private int Hash(string step)
    {
        int currentValue = 0;
        foreach (char c in step.ToCharArray())
        {
            currentValue += (int)c;
            currentValue *= 17;
            currentValue %= 256;
        }
        return currentValue;
    }

    private void PrintStep(string step)
    {
        Console.WriteLine($"After \"{step}\":");
        foreach (int key in Boxes.Keys)
        {
            var boxContent = string.Join(' ', Boxes[key].Select(l => $"[{l.Label} {l.FocalLength}]"));
            Console.WriteLine($"Box {key}: {boxContent}");
        }
        Console.WriteLine("");
    }

    public int HashSum
    {
        get => Steps.Select(Hash).Sum();
    }

    public int FocusingPower
    {
        get => Boxes.Keys.SelectMany(k => Boxes[k].Select((v, i) => (k + 1) * (i + 1) * v.FocalLength)).Sum();
    }
}
