using System.Text.RegularExpressions;

namespace AdventWasm.Solvers;

public class Day01Solver : ISolver
{
    public string Title => "Rotation Counter";
    public string Description => "Count passes through position zero on a circular dial (0-99).";

    private const int MaxPoint = 99;
    private const int MinPoint = 0;
    private const int StartPosition = 50;

    private static List<(string direction, int steps)> ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var rotations = new List<(string direction, int steps)>();

        foreach (var line in lines)
        {
            var match = Regex.Match(line.Trim(), @"([LR])(\d+)");
            if (match.Success)
            {
                rotations.Add((match.Groups[1].Value, int.Parse(match.Groups[2].Value)));
            }
        }

        return rotations;
    }

    public string SolvePart1(string input)
    {
        var rotations = ParseInput(input);
        var position = StartPosition;
        var zeroCounts = 0;

        foreach (var rotation in rotations)
        {
            if (rotation.direction == "L")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position--;
                    if (position < MinPoint) position = MaxPoint;
                }
            }
            else
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position++;
                    if (position > MaxPoint) position = MinPoint;
                }
            }

            if (position == 0) zeroCounts++;
        }

        return zeroCounts.ToString();
    }

    public string SolvePart2(string input)
    {
        var rotations = ParseInput(input);
        var position = StartPosition;
        var zeroCounts = 0;

        foreach (var rotation in rotations)
        {
            if (rotation.direction == "L")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position--;
                    if (position < MinPoint) position = MaxPoint;
                    if (position == 0) zeroCounts++;
                }
            }
            else
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position++;
                    if (position > MaxPoint) position = MinPoint;
                    if (position == 0) zeroCounts++;
                }
            }
        }

        return zeroCounts.ToString();
    }
}
