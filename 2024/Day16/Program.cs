using System.Numerics;
using AdventOfCode.Helpers;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

var solution = new SolutionDay16("input.txt");
solution.PrintSolutions();

public class SolutionDay16 : AbstractSolution
{
    static Complex Up = -Complex.ImaginaryOne;
    static Complex Down = Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;

    public SolutionDay16() : base() { }
    public SolutionDay16(string filepath) : base(filepath) { }

    public override long Part1()
    {
        return 0;
    }

    public override long Part2()
    {
        return 0;
    }

    (Map, Complex startTile, Complex endTile) Parse(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);
        var lines = blocks[0].Split(Environment.NewLine);
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToDictionary();
        return (map, map.Single(x => x.Value == 'S').Key, map.Single(x => x.Value == 'E').Key);
    }
}