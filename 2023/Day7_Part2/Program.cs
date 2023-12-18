using System.ComponentModel;

var lines = File.ReadAllLines("input.txt");
var games = lines.Select(x => x.Split(" ")).Select(x => new Game(x[0], int.Parse(x[1]), GetHandType(x[0])));
var ranks = games.OrderBy(x => x.Type).ThenByDescending(x => x.Hand, new ComparisonComparer()).ToList();
var sum = 0;
for (int i = 1; i < ranks.Count + 1; i++)
{
    var game = ranks[i - 1];
    System.Console.WriteLine("Rank: " + i + "Hand: " + game.Hand + ": " + game.Type + "... Value: " + game.Bid * i);
    sum += game.Bid * i;
}
System.Console.WriteLine(sum);

static HandTypes GetHandType(string hand)
{
    var charGroups = hand.GroupBy(x => x).OrderBy(x => x.ToList().Count);
    if (charGroups.Count() == 1)
    {
        return HandTypes.FiveOfAKind;
    }

    var largestBucket = charGroups.Where(x => x.Key != 'J').Last();
    var count = largestBucket.Count();
    count += hand.Count(x => x == 'J');
    var handType = count switch
    {
        0 => HandTypes.HighCard,
        1 => HandTypes.HighCard,
        2 => HandTypes.OnePair,
        3 => HandTypes.ThreeOfAKind,
        4 => HandTypes.FourOfAKind,
        5 => HandTypes.FiveOfAKind,
        _ => throw new InvalidEnumArgumentException(hand)
    };
    if (handType == HandTypes.OnePair && charGroups.SkipLast(1).Last().Count() == 2)
    {
        handType = HandTypes.TwoPair;
    }

    if (handType == HandTypes.ThreeOfAKind && charGroups.SkipLast(1).Last().Count() == 2)
    {
        handType = HandTypes.FullHouse;
    }

    return handType;
}

public record Game(string Hand, int Bid, HandTypes Type);

public enum HandTypes
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}

public class ComparisonComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        System.Console.WriteLine("Comparing " + x + " and " + y);
        var letterCards = new List<char> { 'T', 'Q', 'K', 'A' };
        for (int i = 0; i < 5; i++)
        {
            var valX = x[i];
            var valY = y[i];
            System.Console.WriteLine("Comparing " + valX + " and " + valY);
            if (valX == valY)
            {
                continue;
            }

            if (valX == 'J')
            {
                return 1;
            }

            if (valY == 'J')
            {
                return -1;
            }

            if (char.IsDigit(valX) && char.IsDigit(valY))
            {
                return int.Parse(valX.ToString()).CompareTo(int.Parse(valY.ToString())) * -1;
            }

            if (char.IsDigit(valX) && !char.IsDigit(valY))
            {
                return 1;
            }

            if (!char.IsDigit(valX) && char.IsDigit(valY))
            {
                return -1;
            }


            if (letterCards.IndexOf(valX) < letterCards.IndexOf(valY))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        return 0;
    }
}