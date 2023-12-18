using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var numberRegex = new Regex(@"\d+");
var sum = 0;
foreach (var line in lines)
{
    var parts = line.Split("|");
    var gameNumbers = numberRegex.Matches(parts[0].Split(":")[1]).Select(x => int.Parse(x.Value));
    var numbers = numberRegex.Matches(parts[1]).Select(x => int.Parse(x.Value));
    var matches = numbers.Intersect(gameNumbers).ToList();
    var value = 0;
    foreach (var match in matches)
    {
        if (value == 0)
        {
            value++;
        }
        else
        {
            value *= 2;
        }
    }
    sum += value;
}
System.Console.WriteLine(sum);