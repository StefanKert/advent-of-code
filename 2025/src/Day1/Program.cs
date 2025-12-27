using System.Text.RegularExpressions;

var maxPoint = 99;
var minPoint = 0;

// Support both file input and WASM (stdin/env) input
string[] lines;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    lines = aocInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else if (Console.IsInputRedirected)
{
    lines = Console.In.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else
{
    lines = File.ReadAllLines("input.txt");
}

var rotations = lines.Select(x => (direction: Regex.Match(x, @"([LR])(\d+)").Groups[1].Value, steps: int.Parse(Regex.Match(x, @"([LR])(\d+)").Groups[2].Value))).ToList();

// Check if running specific part (for WASM)
var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(CountPassTozero(maxPoint, minPoint, rotations));
}
else if (partStr == "2")
{
    Console.WriteLine(CountAllPassingsOfZero(maxPoint, minPoint, rotations));
}
else
{
    // Normal execution - run both parts
    int counts = CountPassTozero(maxPoint, minPoint, rotations);
    Console.WriteLine($"Part 1: Times at zero: {counts}");
    int zeroCounts = CountAllPassingsOfZero(maxPoint, minPoint, rotations);
    Console.WriteLine($"Part 2: Total times at zero: {zeroCounts}");
}

static int CountPassTozero(int maxPoint, int minPoint, List<(string direction, int steps)> rotations)
{
    var position = 50;
    var zeroCounts = 0;
    for (int i = 0; i < rotations.Count; i++)
    {
        var rotation = rotations[i];
        if (rotation.direction == "L")
        {
            for (int j = 0; j < rotation.steps; j++)
            {
                position--;
                if (position < minPoint) position = maxPoint;
            }
        }
        else if (rotation.direction == "R")
        {
            for (int j = 0; j < rotation.steps; j++)
            {
                position++;
                if (position > maxPoint) position = minPoint;
            }
        }
        if (position == 0) zeroCounts++;
    }
    return zeroCounts;
}

static int CountAllPassingsOfZero(int maxPoint, int minPoint, List<(string direction, int steps)> rotations)
{
    var position = 50;
    var zeroCounts = 0;
    for (int i = 0; i < rotations.Count; i++)
    {
        var rotation = rotations[i];
        if (rotation.direction == "L")
        {
            for (int j = 0; j < rotation.steps; j++)
            {
                position--;
                if (position < minPoint) position = maxPoint;
                if (position == 0) zeroCounts++;
            }
        }
        else if (rotation.direction == "R")
        {
            for (int j = 0; j < rotation.steps; j++)
            {
                position++;
                if (position > maxPoint) position = minPoint;
                if (position == 0) zeroCounts++;
            }
        }
    }
    return zeroCounts;
}
