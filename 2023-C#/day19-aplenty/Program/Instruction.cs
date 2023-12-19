namespace Namespace;

public class Instruction
{
    public record Check(string Category, char Operator, int Value, string Return);
    public readonly string Name;
    public readonly string FallBackReturn;
    public readonly List<Check> Checks;
    public Instruction(string input)
    {
        // This parses name1{s<\d+:name2,x>\d+:R,A}
        var rawInput = input.Split('{');
        Name = rawInput.First();
        var rawInputRules = rawInput.Last();
        var rawRules = rawInputRules[0..(rawInputRules.Length - 1)].Split(',').ToList();
        FallBackReturn = rawRules.Last();
        rawRules.RemoveAt(rawRules.Count - 1);
        Checks = rawRules.Select(r =>
        {
            var ruleOperator = r.Contains('>') ? '>' : '<';
            var next = r.Split(ruleOperator);
            var ruleCategory = next.First();
            next = next.Last().Split(':');
            var ruleValue = int.Parse(next.First());
            var ruleReturn = next.Last();
            return new Check(ruleCategory, ruleOperator, ruleValue, ruleReturn);
        }).ToList();

        // Small optim
        if (Checks.All(c => c.Return == FallBackReturn))
        {
            Checks = new();
        }
    }
}
