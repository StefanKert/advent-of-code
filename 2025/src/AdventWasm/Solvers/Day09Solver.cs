namespace AdventWasm.Solvers;

public class Day09Solver : ISolver
{
    public string Title => "2D Distance Calculation";
    public string Description => "Find largest distance pair and calculate bounding box area.";

    private static List<(long x, long y)> ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.Select(x => (x: long.Parse(x.Split(',')[0].Trim()), y: long.Parse(x.Split(',')[1].Trim()))).ToList();
    }

    private static double Distance2D((long x, long y) point1, (long x, long y) point2)
    {
        return Math.Sqrt(
            Math.Pow(point2.x - point1.x, 2) +
            Math.Pow(point2.y - point1.y, 2)
        );
    }

    private static List<(double distance, (long x, long y) point1, (long x, long y) point2)> CalculateDistances(List<(long x, long y)> dataPoints)
    {
        var distances = new List<(double distance, (long x, long y) point1, (long x, long y) point2)>();
        var seen = new HashSet<(int, int)>();

        for (int i = 0; i < dataPoints.Count; i++)
        {
            for (int j = 0; j < dataPoints.Count; j++)
            {
                if (i == j) continue;

                var a = Math.Min(i, j);
                var b = Math.Max(i, j);

                if (!seen.Add((a, b))) continue;

                var d = Distance2D(dataPoints[i], dataPoints[j]);
                distances.Add((d, dataPoints[i], dataPoints[j]));
            }
        }

        return distances;
    }

    public string SolvePart1(string input)
    {
        var coordinates = ParseInput(input);
        var distances = CalculateDistances(coordinates).OrderByDescending(x => x.distance).ToList();

        if (distances.Count == 0) return "0";

        var largestDistance = distances[0];
        long left = Math.Min(largestDistance.point1.x, largestDistance.point2.x);
        long right = Math.Max(largestDistance.point1.x, largestDistance.point2.x);
        long top = Math.Max(largestDistance.point1.y, largestDistance.point2.y);
        long bottom = Math.Min(largestDistance.point1.y, largestDistance.point2.y);
        long width = right - left + 1;
        long height = top - bottom + 1;

        return (width * height).ToString();
    }

    public string SolvePart2(string input)
    {
        var coordinates = ParseInput(input);
        var distances = CalculateDistances(coordinates).OrderByDescending(x => x.distance).ToList();

        if (distances.Count == 0) return "0";

        var largestDistance = distances[0];
        var squareOfLargestDistance = Math.Abs(largestDistance.point1.x - largestDistance.point2.x - 1) *
                                      Math.Abs(largestDistance.point1.y - largestDistance.point2.y - 1);

        return squareOfLargestDistance.ToString();
    }
}
