using System.Text.RegularExpressions;

namespace Day1Wasm;

/// <summary>
/// WASI Component implementation for Advent of Code 2025 Day 1
/// </summary>
public class Day1SolverImpl : AdventExports.Advent.Day1.ISolver
{
    private const int MaxPoint = 99;
    private const int MinPoint = 0;
    private const int StartPosition = 50;

    /// <summary>
    /// Parse input string into rotation instructions
    /// </summary>
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

    /// <summary>
    /// Part 1: Count how many times we end at position 0 after each rotation
    /// </summary>
    public int SolvePart1(string input)
    {
        var rotations = ParseInput(input);
        return CountPassToZero(rotations);
    }

    /// <summary>
    /// Part 2: Count every individual pass through position 0
    /// </summary>
    public int SolvePart2(string input)
    {
        var rotations = ParseInput(input);
        return CountAllPassingsOfZero(rotations);
    }

    /// <summary>
    /// Solve both parts and return formatted string
    /// </summary>
    public string SolveAll(string input)
    {
        var part1 = SolvePart1(input);
        var part2 = SolvePart2(input);
        return $"Part 1: {part1}\nPart 2: {part2}";
    }

    private static int CountPassToZero(List<(string direction, int steps)> rotations)
    {
        var position = StartPosition;
        var zeroCounts = 0;

        foreach (var rotation in rotations)
        {
            if (rotation.direction == "L")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position--;
                    if (position < MinPoint)
                    {
                        position = MaxPoint;
                    }
                }
            }
            else if (rotation.direction == "R")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position++;
                    if (position > MaxPoint)
                    {
                        position = MinPoint;
                    }
                }
            }

            if (position == 0)
            {
                zeroCounts++;
            }
        }

        return zeroCounts;
    }

    private static int CountAllPassingsOfZero(List<(string direction, int steps)> rotations)
    {
        var position = StartPosition;
        var zeroCounts = 0;

        foreach (var rotation in rotations)
        {
            if (rotation.direction == "L")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position--;
                    if (position < MinPoint)
                    {
                        position = MaxPoint;
                    }
                    if (position == 0)
                    {
                        zeroCounts++;
                    }
                }
            }
            else if (rotation.direction == "R")
            {
                for (int j = 0; j < rotation.steps; j++)
                {
                    position++;
                    if (position > MaxPoint)
                    {
                        position = MinPoint;
                    }
                    if (position == 0)
                    {
                        zeroCounts++;
                    }
                }
            }
        }

        return zeroCounts;
    }
}
