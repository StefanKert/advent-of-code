using AdventOfCode.Helpers;
using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

var solution = new SolutionDay10();
solution.PrintSolutions();

public class SolutionDay10 : AbstractSolution
{
    Complex Up = Complex.ImaginaryOne;
    Complex Down = -Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    public SolutionDay10() : base() { }
    public SolutionDay10(string filepath) : base(filepath) { }

    public override long Part1()
    {
        (var map, var trailHeads) = GetMap(_input);
        var trails = trailHeads.Select(t => GetTrailsFrom(map, t));
        return trails.Sum(t => t.Distinct().Count());
    }

    public override long Part2()
    {
        (var map, var trailHeads) = GetMap(_input);
        var trails = trailHeads.Select(t => GetTrailsFrom(map, t)); 
        return trails.Sum(t => t.Count);
    }

    private List<Complex> GetTrailsFrom(Map map, Complex trailHead)
    {
        var positions = new Queue<Complex>();
        positions.Enqueue(trailHead);
        var trails = new List<Complex>();
        while (positions.Count > 0)
        {
            var point = positions.Dequeue();
            if (map[point] == '9')
            {
                trails.Add(point);
            }
            else
            {
                foreach (var dir in new[] { Up, Down, Left, Right })
                {
                    if (map.GetValueOrDefault(point + dir) == map[point] + 1)
                    {
                        positions.Enqueue(point + dir);
                    }
                }
            }
        }
        return trails;
    }

    (Map map, IEnumerable<Complex> trailHeads) GetMap(string input)
    {
        var data = input.Split(Environment.NewLine);
        var m = (
            from y in Enumerable.Range(0, data.Length)
            from x in Enumerable.Range(0, data[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, data[y][x])
        ).ToImmutableDictionary();
        return (m, m.Keys.Where(pos => m[pos] == '0'));
    }
}
