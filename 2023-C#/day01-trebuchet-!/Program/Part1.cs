namespace Namespace;

public class Part1
{
    private readonly string Input;
    public Part1(string input) => Input = input;
    public int Solve() => new Trebuchet(Input).CalibrationValuesSum(@"\d");
}
