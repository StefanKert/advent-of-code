using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;
using Position = (long X, long Y);

var solution = new SolutionDay13("input.txt");
solution.PrintSolutions();

public class SolutionDay13 : AbstractSolution
{
    public int TOKENS_BUTTON_A = 3;
    public int TOKENS_BUTTON_B = 1;

    public SolutionDay13() : base() { }
    public SolutionDay13(string filepath) : base(filepath) { }

    public override long Part1()
    {
        var games = GetGames(_input);
        var sum = 0L;
        foreach (var game in games)
        {
            sum += GetTokensForGame(game);
        }
        return sum;
    }

    private long GetTokensForGame(Game game)
    {
        // Cramers Rule https://en.wikipedia.org/wiki/Cramer%27s_rule
        // y = game.ButtonB.Y * i + game.ButtonA.Y * j 
        // x = game.ButtonB.X * i + game.ButtonA.X * j
        var a1 = game.ButtonA.X;
        var b1 = game.ButtonB.X;
        var a2 = game.ButtonA.Y;
        var b2 = game.ButtonB.Y;
        var c1 = game.Price.X;
        var c2 = game.Price.Y;
        // a1*x + b1*y = c1
        // a2*x + b2*y = c2
        // |a1 b1| |x|   |c1|
        // |a2 b2| |y| = |c2|  
        var x = (c1 * b2 - b1 * c2) / (a1 * b2 - b1 * a2);
        var y = (a1 * c2 - c1 * a2) / (a1 * b2 - b1 * a2);
        if (game.ButtonA.X * x + game.ButtonB.X * y != game.Price.X && game.ButtonA.Y * x + game.ButtonB.Y * y != game.Price.Y)
        {
            return 0;
        }

        // return the prize when a non negative _integer_ solution is found
        if (x >= 0 && y >= 0)
        {
            return TOKENS_BUTTON_A * x + TOKENS_BUTTON_B * y;
        }
        return 0;
    }

    public override long Part2()
    {
        var games = GetGames(_input);
        var sum = 0L;
        foreach (var game in games)
        {
            game.Price = (game.Price.X + 10000000000000, game.Price.Y + 10000000000000);
            sum += GetTokensForGame(game);
        }
        return sum;
    }

    public List<Game> GetGames(string input)
    {
        var games = new List<Game>();
        var gamesRaw = input.Split(Environment.NewLine + Environment.NewLine);
        foreach (var gameRaw in gamesRaw)
        {
            var game = new Game();
            var gameData = gameRaw.Split(Environment.NewLine);
            var regex = new Regex(@"\d+");
            var match = regex.Matches(gameData[0]);
            game.ButtonA = new Position { X = int.Parse(match[0].Value), Y = int.Parse(match[1].Value) };
            var match1 = regex.Matches(gameData[1]);
            game.ButtonB = new Position { X = int.Parse(match1[0].Value), Y = int.Parse(match1[1].Value) };
            var match2 = regex.Matches(gameData[2]);
            game.Price = new Position { X = int.Parse(match2[0].Value), Y = int.Parse(match2[1].Value) };
            games.Add(game);
        }
        return games;
    }
}

public record Game
{
    public Position ButtonA { get; set; }
    public Position ButtonB { get; set; }
    public Position Price { get; set; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Button A: X+{ButtonA.X}, Y+{ButtonA.Y}");
        stringBuilder.AppendLine($"Button B: X+{ButtonB.X}, Y+{ButtonB.Y}");
        stringBuilder.AppendLine($"Price: X={Price.X}, Y={Price.Y}");
        return stringBuilder.ToString();
    }
}