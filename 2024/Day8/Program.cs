using AdventOfCode.Helpers;
using System.Collections.Immutable;
using System.Numerics;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

var solution = new SolutionDay8();
solution.PrintSolutions();

public class SolutionDay8 : AbstractSolution
{
    public SolutionDay8() : base() { }
    public SolutionDay8(string filepath) : base(filepath) { }

    public override long Part1()
    {
        var map = GetMap(_input);
        var antennaLocations = GetAntennas(map);
        var antennaPairs = GetAntennaPairs(antennaLocations, map);
        var data = new HashSet<Complex>();
        foreach (var (srcAntenna, dstAntenna) in antennaPairs)
        {
            var dir = dstAntenna - srcAntenna;
            var antinote = dstAntenna + dir;
            if (map.Keys.Contains(antinote))
            {
                data.Add(antinote);
            }
        }
        return data.ToHashSet().Count;
    }

    public override long Part2()
    {
        var map = GetMap(_input);
        var antennaLocations = GetAntennas(map);
        var antennaPairs = GetAntennaPairs(antennaLocations, map);
        var data = new HashSet<Complex>();
        foreach (var (srcAntenna, dstAntenna) in antennaPairs)
        {
            var dir = dstAntenna - srcAntenna;
            var antinote = dstAntenna;
            while (map.Keys.Contains(antinote))
            {
                data.Add(antinote);
                antinote += dir;
            }
        }
        return data.ToHashSet().Count;
    }

    List<(Complex srcAntenna, Complex dstAntenna)> GetAntennaPairs(List<Complex> antennaLocations, Map map)
    {
        var data = new List<(Complex srcAntenna, Complex dstAntenna)>();
        for (int i = 0; i < antennaLocations.Count; i++)
        {
            for (int j = 0; j < antennaLocations.Count; j++)
            {
                var srcAntenna = antennaLocations[i];
                var dstAntenna = antennaLocations[j];
                if (map[srcAntenna] == map[dstAntenna])
                {
                    if (srcAntenna == dstAntenna)
                    {
                        continue;
                    }
                    data.Add((srcAntenna, dstAntenna));
                }
            }
        }
        return data;
    }

    private static List<Complex> GetAntennas(Map map)
    {
        var data = new List<Complex>();
        foreach (var pos in map.Keys)
        {
            if (char.IsAsciiLetterOrDigit(map[pos]))
            {
                data.Add(pos);
            }
        }
        return data;
    }

    Map GetMap(string input)
    {
        var map = input.Split(Environment.NewLine);
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x - y * Complex.ImaginaryOne, map[y][x])
        ).ToImmutableDictionary();
    }
}
