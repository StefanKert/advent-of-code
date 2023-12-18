﻿
var basket = new Dictionary<string, int> {
   { "red", 12 },
   { "blue",  13},
   { "green",  14},
};
var sum = 0;
var lines = File.ReadAllLines("input.txt").ToList();
foreach (var line in lines)
{
    var maxRed = 0;
    var maxBlue = 0;
    var maxGreen = 0;
    var setValid = true;
    var parts = line.Split(":");
    var gameIdentifier = parts[0].Replace("Game ", "");
    var pulls = parts[1];
    var sets = pulls.Split(";");
    foreach (var set in sets)
    {
        var red = 0;
        var blue = 0;
        var green = 0;
        var draws = set.Split(",");
        foreach (var draw in draws)
        {
            if (draw.EndsWith("red"))
            {
                red += int.Parse(draw.Replace("red", "").Trim());
            }

            if (draw.EndsWith("blue"))
            {
                blue += int.Parse(draw.Replace("blue", "").Trim());
            }

            if (draw.EndsWith("green"))
            {
                green += int.Parse(draw.Replace("green", "").Trim());
            }
        }

        if (red > maxRed)
        {
            maxRed = red;
        }

        if (blue > maxBlue)
        {
            maxBlue = blue;
        }

        if (green > maxGreen)
        {
            maxGreen = green;
        }
    }
    var power = maxBlue * maxGreen * maxRed;
    System.Console.WriteLine($"Game {gameIdentifier} {line}... red {maxRed}, green {maxGreen}, blue {maxBlue}: {power}");
    sum += power;
}
System.Console.WriteLine(sum);
