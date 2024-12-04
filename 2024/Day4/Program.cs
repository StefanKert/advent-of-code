using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

Complex Up = -Complex.ImaginaryOne;
Complex Down = Complex.ImaginaryOne;
Complex Left = -1;
Complex Right = 1;

Console.WriteLine($"Part 1: XMAS Times {PartOne(File.ReadAllText("input.txt"))}");
Console.WriteLine($"Part 2: MAS Cross {PartTwo(File.ReadAllText("input.txt"))}");

Map GetMap(string input)
{
    var map = input.Split(Environment.NewLine);
    return (
        from y in Enumerable.Range(0, map.Length)
        from x in Enumerable.Range(0, map[0].Length)
        select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, map[y][x])
    ).ToImmutableDictionary();
}

long PartOne(string input)
{
    var pattern = "XMAS";
    var map = GetMap(input);
    var xmasTimes = 0;
    foreach (var key in map.Keys)
    {
        foreach (var direction in new[] { Right, Right + Down, Down + Left, Down })
        {
            if(Match(pattern, map, xmasTimes, key, direction))
            {
                xmasTimes++;
            }
        }
    }
    return xmasTimes;
}

long PartTwo(string input)
{
    var pattern = "MAS";
    var map = GetMap(input);
    var xmasTimes = 0;
    foreach (var key in map.Keys)
    {
        var point = key + Up + Left;
        var direction = Down + Right;
        var chars = Enumerable.Range(0, pattern.Length)
                  .Select(i => map.GetValueOrDefault(point + i * direction))
                  .ToArray();
        if (Enumerable.SequenceEqual(chars, pattern) || Enumerable.SequenceEqual(chars, pattern.Reverse()))
        {
            var innerPoint = key + Down + Left;
            var innerDirection = Up + Right;
            var innerChars = Enumerable.Range(0, pattern.Length)
                      .Select(i => map.GetValueOrDefault(innerPoint + i * innerDirection))
                      .ToArray();
            if (Enumerable.SequenceEqual(innerChars, pattern) || Enumerable.SequenceEqual(innerChars, pattern.Reverse()))
            {
                xmasTimes++;
            }
        }
    }
    return xmasTimes;
}

static bool Match(string pattern, Map map, int xmasTimes, Complex key, Complex direction)
{
    var next = key + direction;
    var chars = Enumerable.Range(0, pattern.Length)
           .Select(i => map.GetValueOrDefault(key + i * direction))
           .ToArray();
    return Enumerable.SequenceEqual(chars, pattern) || Enumerable.SequenceEqual(chars, pattern.Reverse());
}