namespace Namespace;

public class Game
{
    private static readonly char[] CardsWithJokers = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    private readonly List<Hand> Hands;
    public Game(string input, bool withJokers = false)
    {
        if (withJokers)
        {
            Hands = new();
            foreach (string handInput in input.Split('\n'))
            {
                if (!handInput.Contains('J'))
                {
                    Hands.Add(new Hand(handInput, withJokers));
                    continue;
                }
                var bestHand = new Hand(handInput.Replace('J', '2'), withJokers, handInput);
                foreach (char replacement in CardsWithJokers)
                {
                    var newHand = new Hand(handInput.Replace('J', replacement), withJokers, handInput);
                    if (newHand.CompareTo(bestHand) > 0)
                    {
                        bestHand = newHand;
                    }
                }
                Hands.Add(bestHand);
            }
        }
        else
        {
            Hands = input.Split('\n').Select(s => new Hand(s)).ToList();
        }
        Hands.Sort();
    }

    public int TotalWinnings
    {
        get => Hands.Select(h => (Hands.IndexOf(h) + 1) * h.Bid).Sum();
    }
}
