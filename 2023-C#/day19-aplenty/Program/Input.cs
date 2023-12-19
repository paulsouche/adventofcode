namespace Namespace;

public class Input
{
    public readonly Dictionary<string, int> Categories;

    public Input(string input)
    {
        // This parses {x=\d+,m=\d+,a=\d+,s=\d+}
        Categories = input[1..(input.Length - 1)].Split(',').Select(i => i.Split('=')).ToDictionary(arr => arr[0], arr => int.Parse(arr[1]));
    }
    public bool IsAccepted(Dictionary<string, Instruction> instructions)
    {
        Instruction? instruction = instructions["in"];
        string instructionReturn = "R";
        while (instruction != null)
        {
            var check = instruction.Checks.Find(c => c.Operator == '>' ? Categories[c.Category] > c.Value : Categories[c.Category] < c.Value);
            instructionReturn = check != null ? check.Return : instruction.FallBackReturn;
            instruction = instructions.ContainsKey(instructionReturn) ? instructions[instructionReturn] : null;
        }
        return instructionReturn == "A";
    }

    public int RatingNumber
    {
        get => Categories.Values.Sum();
    }
}
