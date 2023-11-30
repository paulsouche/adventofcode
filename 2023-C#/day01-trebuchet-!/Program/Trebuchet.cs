using System.Text.RegularExpressions;

namespace Namespace;

class Trebuchet
{
    private static int ParseMatch(string s) => s switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        var d => int.Parse(d)
    };

    private readonly string[] Lines;

    public Trebuchet(string input) => Lines = input.Split("\n");

    public int CalibrationValuesSum(string rx) => Lines.Select(l =>
    {
        var first = Regex.Match(l, rx);
        var last = Regex.Match(l, rx, RegexOptions.RightToLeft);
        return ParseMatch(first.Value) * 10 + ParseMatch(last.Value);
    }).Sum();
}
