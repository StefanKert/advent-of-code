using System.Text.RegularExpressions;

// Read part number from environment or args
var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

// Read input from environment or stdin
var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input))
{
    input = Console.In.ReadToEnd();
}

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

const int MaxPoint = 99;
const int MinPoint = 0;
const int StartPosition = 50;

static List<(string direction, int steps)> ParseInput(string input)
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

static string SolvePart1(string input)
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

static string SolvePart2(string input)
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
