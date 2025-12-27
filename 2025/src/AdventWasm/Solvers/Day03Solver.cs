namespace AdventWasm.Solvers;

public class Day03Solver : ISolver
{
    public string Title => "Battery Aggregation";
    public string Description => "Recursively aggregate battery values by finding max in remaining range.";

    private static List<List<long>> ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.Select(line => line.Select(c => long.Parse(c.ToString())).ToList()).ToList();
    }

    private static long Aggregate(List<long> batteries, int batteriesEnabled)
    {
        if (batteriesEnabled == 0 || batteries.Count == 0) return 0;

        var rangeEnd = batteries.Count - (batteriesEnabled - 1);
        if (rangeEnd <= 0) rangeEnd = batteries.Count;

        var maxVal = batteries.Take(rangeEnd).Max();
        var maxIdx = batteries.Take(rangeEnd).ToList().IndexOf(maxVal);

        var power = (long)Math.Pow(10, batteriesEnabled - 1);
        var contribution = maxVal * power;

        var remaining = batteries.Skip(maxIdx + 1).ToList();
        return contribution + Aggregate(remaining, batteriesEnabled - 1);
    }

    public string SolvePart1(string input)
    {
        var grid = ParseInput(input);
        var total = grid.Sum(row => Aggregate(row, 2));
        return total.ToString();
    }

    public string SolvePart2(string input)
    {
        var grid = ParseInput(input);
        var total = grid.Sum(row => Aggregate(row, 12));
        return total.ToString();
    }
}
