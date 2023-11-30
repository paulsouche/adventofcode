namespace Namespace;

public class Part2
{
    private readonly string Input;
    public Part2(string input) => Input = input;
    public int Solve() => new Trebuchet(Input).CalibrationValuesSum(@"\d|one|two|three|four|five|six|seven|eight|nine");
}
