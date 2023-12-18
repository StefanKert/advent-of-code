using System.Text.RegularExpressions;

var numberRegex = new Regex(@"\d+");
var gameNumberRegex = new Regex(@"(?<=Card\s+)\d+");
var winningNumbersRegex = new Regex(@"(?<=:.*?)(\d+)(?=.*\|)");
var numbersRegex = new Regex(@"(?<=\|.*?)(\d+)");

var wonGames = new Dictionary<int, int>();

var lines = File.ReadAllLines("input.txt");
var games = new List<Game>();
foreach (var line in lines)
{
    var parts = line.Split("|");
    var gameString = parts[0].Split(":")[0];

    var winningNumbersString = parts[0].Split(":")[1];
    var gameNumber = int.Parse(gameNumberRegex.Match(line).Value);
    var winningNumbers = winningNumbersRegex.Matches(line).Select(x => int.Parse(x.Value));
    var numbers = numbersRegex.Matches(line).Select(x => int.Parse(x.Value));
    games.Add(new Game(gameNumber, numbers.Intersect(winningNumbers).ToList().Count));
}

var sum = 0;
foreach (var game in games)
{
    sum += WinCardsForGame(game);
}

System.Console.WriteLine(sum);

int WinCardsForGame(Game game)
{
    if (wonGames.ContainsKey(game.game) == false)
    {
        wonGames.Add(game.game, 0);
    }
    wonGames[game.game] += 1;

    if (game.matches == 0)
    {
        return 1;
    }
    var wonCards = 1;
    for (var i = 1; i <= game.matches; i++)
    {
        var nextGame = games.FirstOrDefault(x => x.game == game.game + i);
        if (nextGame == null)
        {
            break;
        }
        wonCards += WinCardsForGame(nextGame);
    }
    return wonCards;
}
System.Console.WriteLine(wonGames.Sum(x => x.Value));
public record Game(int game, int matches);

