using System.Text.RegularExpressions;

var matches = Regex.Matches(File.ReadAllText("input.txt"), @"mul\((\d+),(\d+)\)").Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum();
var switchOffMatches = Regex.Matches(File.ReadAllText("input.txt"), @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

var enabled = true;
var sum = 0;
foreach (Match match in switchOffMatches)
{
    if (match.ToString() == "don't()")
    {
        enabled = false;
    }
    else if (match.ToString() == "do()")
    {
        enabled = true;
    }
    else
    {
        if (enabled)
        {
            sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }
    }
}

Console.WriteLine($"Part 1: Multiplications {matches}");
Console.WriteLine($"Part 2: Multiplications (switch-off) {sum}");