using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

var solution = new SolutionDay3();
solution.PrintSolutions();

public class SolutionDay3 : AbstractSolution
{
    private string _input;

    public SolutionDay3()
    {
        _input = File.ReadAllText("input.txt");
    }

    public override long Part1()
    {
        return Regex.Matches(_input, @"mul\((\d+),(\d+)\)").Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum();
    }

    public override long Part2()
    {
        var matches = Regex.Matches(_input, @"mul\((\d+),(\d+)\)").Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum();
        var switchOffMatches = Regex.Matches(_input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");
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
        return sum;
    }
}