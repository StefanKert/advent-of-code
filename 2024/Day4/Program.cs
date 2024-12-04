using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

Complex Up = -Complex.ImaginaryOne;
Complex Down = Complex.ImaginaryOne;
Complex Left = -1;
Complex Right = 1;

var xmasTimes = 0;

Console.WriteLine($"Part 1: XMAS Times {xmasTimes}");
//Console.WriteLine($"Part 2: Multiplications (switch-off) {sum}");

Map GetMap(string input)
{
    var map = input.Split(Environment.NewLine);
    return (
        from y in Enumerable.Range(0, map.Length)
        from x in Enumerable.Range(0, map[0].Length)
        select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, map[y][x])
    ).ToImmutableDictionary();
}