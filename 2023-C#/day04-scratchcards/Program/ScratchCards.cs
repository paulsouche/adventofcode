namespace Namespace;

public class ScratchCards
{
    private readonly List<Card> Cards;
    private readonly List<int> CardsCount;

    public ScratchCards(string input)
    {
        Cards = input.Split('\n').Select(i => new Card(i)).ToList();
        CardsCount = Cards.Select(c => 1).ToList();

        for (int i = 0; i < Cards.Count; i++)
        {
            var winningNumbersCount = Cards[i].WinningNumbersCount;
            for (int j = i + 1; j < i + 1 + winningNumbersCount; j++)
            {
                CardsCount[j] += CardsCount[i];
            }
        }
    }

    public double Points
    {
        get => Cards.Select(c => c.Worth).Sum();
    }

    public int TotalCards
    {
        get => CardsCount.Sum();
    }
}
