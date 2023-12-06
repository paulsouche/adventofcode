namespace Namespace;

public class Hand : IComparable<Hand>
{
    private static readonly Dictionary<char, int> RawCardsValuesWithJoker;
    private static readonly Dictionary<char, int> RawCardsValues;
    static Hand()
    {
        RawCardsValues = new()
        {
            {'2', 1},
            {'3', 2},
            {'4', 3},
            {'5', 4},
            {'6', 5},
            {'7', 6},
            {'8', 7},
            {'9', 8},
            {'T', 9},
            {'J', 10},
            {'Q', 11},
            {'K', 12},
            {'A', 13},
        };

        RawCardsValuesWithJoker = new()
        {
            {'J', 1},
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'Q', 11},
            {'K', 12},
            {'A', 13},
        };
    }

    private readonly char[] RawCards;
    private readonly Dictionary<char, int> Cards;
    private readonly bool WithJokers;

    public readonly int Bid;
    public Hand(string input, bool withJokers = false, string? originalInput = null)
    {
        var rawHand = input.Split(' ');
        WithJokers = withJokers;
        RawCards = rawHand.First().ToCharArray();
        Cards = new();
        foreach (char card in rawHand.First())
        {
            Cards[card] = Cards.ContainsKey(card) ? Cards[card] + 1 : 1;
        }
        Bid = int.Parse(rawHand.Last());
        if (originalInput != null)
        {
            RawCards = originalInput.Split(' ').First().ToCharArray();
        }
    }

    public bool IsFiveOfAKind
    {
        get => Cards.Keys.Count == 1;
    }

    public bool IsFourOfAKind
    {
        get => Cards.Keys.Count == 2 && Cards.Values.Any(v => v == 4);
    }

    public bool IsFullHouse
    {
        get => !IsFourOfAKind && Cards.Keys.Count == 2;
    }

    public bool IsThreeOfAKind
    {
        get => Cards.Keys.Count == 3 && Cards.Values.Any(v => v == 3);
    }

    public bool IsTwoPair
    {
        get => !IsThreeOfAKind && Cards.Keys.Count == 3;
    }

    public bool IsPair
    {
        get => Cards.Keys.Count == 4;
    }

    private int Power
    {
        get
        {
            if (IsFiveOfAKind)
            {
                return 6;
            }
            else if (IsFourOfAKind)
            {
                return 5;
            }
            else if (IsFullHouse)
            {
                return 4;
            }
            else if (IsThreeOfAKind)
            {
                return 3;
            }
            else if (IsTwoPair)
            {
                return 2;
            }
            else if (IsPair)
            {
                return 1;
            }
            return 0;
        }
    }

    public int CompareTo(Hand? otherHand)
    {
        if (otherHand == null)
        {
            return 1;
        }

        if (Power != otherHand.Power)
        {
            return Power > otherHand.Power ? 1 : -1;
        }

        int i = 0;
        int result = 0;
        while (i < RawCards.Length)
        {
            var value = WithJokers ? RawCardsValuesWithJoker[RawCards[i]] : RawCardsValues[RawCards[i]];
            var otherValue = WithJokers ? RawCardsValuesWithJoker[otherHand.RawCards[i]] : RawCardsValues[otherHand.RawCards[i]];
            if (value != otherValue)
            {
                result = value > otherValue ? 1 : -1;
                break;
            }
            i++;
        }
        return result;
    }
}
