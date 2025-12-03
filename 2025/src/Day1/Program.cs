using System.ComponentModel;
using System.Text.RegularExpressions;


var maxPoint = 99;
var minPoint = 0;
var rotaionIndicators = new List<string> { "L", "R" };

var rotations = File.ReadAllLines("input.txt").Select(x => (direction: Regex.Match(x, @"([LR])(\d+)").Groups[1].Value, steps: int.Parse(Regex.Match(x, @"([LR])(\d+)").Groups[2].Value))).ToList();

// Part 1
int counts = CountPassTozero(maxPoint, minPoint, rotations);
Console.WriteLine($"Part 1: Times at zero: {counts}");
// Part 2
int zeroCounts = CountAllPassingsOfZero(maxPoint, minPoint, rotations);
Console.WriteLine($"Part 2: Total times at zero: {zeroCounts}");
Console.ReadLine();


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
                if (position < minPoint)
                {
                    position = maxPoint;
                }
            }
        }
        else if (rotation.direction == "R")
        {
            for (int j = 0; j < rotation.steps; j++)
            {
                position++;
                if (position > maxPoint)
                {
                    position = minPoint;
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
                if (position < minPoint)
                {
                    position = maxPoint;
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
                if (position > maxPoint)
                {
                    position = minPoint;
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