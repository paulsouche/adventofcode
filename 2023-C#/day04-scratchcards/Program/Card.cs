namespace Namespace;

public class Card
{
    public readonly int WinningNumbersCount;

    public Card(string input)
    {
        var rawNumbers = input.Split(": ").Last().Split(" | ").ToList();
        var winningNumbers = rawNumbers.First().Split(' ').Where(s => s != "").Select(int.Parse).ToHashSet();
        var numbers = rawNumbers.Last().Split(' ').Where(s => s != "").Select(int.Parse).ToHashSet();
        WinningNumbersCount = numbers.Where(n => winningNumbers.Contains(n)).Count();
    }

    public double Worth
    {
        get => WinningNumbersCount == 0 ? 0 : Math.Pow(2, WinningNumbersCount - 1);
    }
}
