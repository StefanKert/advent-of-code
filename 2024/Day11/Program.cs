using AdventOfCode.Helpers;
using System.Globalization;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(long, int), long>;

var solution = new SolutionDay11();
solution.PrintSolutions();

public class SolutionDay11 : AbstractSolution
{
    public SolutionDay11() : base() { }
    public SolutionDay11(string filepath) : base(filepath) { }

    public override long Part1()
    {
        return CountStones(25);
    }

    public override long Part2()
    {
        return CountStones(75);
    }

    public long CountStones(int blinks)
    {
        var cache = new Cache();
        var sum = 0L;
        foreach (var stone in GetStones(_input))
        {
            sum += Blink(stone, blinks, cache);
        }
        return sum;
    }

    public long Blink(long stone, int blinks, Cache cache)
    {
        return cache.GetOrAdd((stone, blinks), key =>
        {
            if (key.Item2 == 0)
            {
                return 1;
            }
            else if (key.Item1 == 0)
            {
                return Blink(1, blinks - 1, cache);
            }
            else if (key.Item1.ToString().Length % 2 == 0)
            {
                var stoneData = stone.ToString();
                return Blink(long.Parse(stoneData[0..(stoneData.Length / 2)]), blinks - 1, cache) + Blink(long.Parse(stoneData[(stoneData.Length / 2)..]), blinks - 1, cache);
            }
            else
            {
                return Blink(stone * 2024, blinks - 1, cache);
            }
        });
    }

    public List<long> GetStones(string input)
    {
        return input.Split(' ').Select(long.Parse).ToList();
    }
}
