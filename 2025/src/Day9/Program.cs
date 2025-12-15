using System.Drawing;

var coordinates = File.ReadAllLines("input.txt").Select(x => (x: long.Parse(x.Split(',')[0]), y: long.Parse(x.Split(',')[1]))).ToList();
var distances = CalculateDistances(coordinates).OrderByDescending(x => x.distance).ToList();

var largestDistance = distances[0];
var squareOfLargestDistance = Math.Abs(largestDistance.point1.x - largestDistance.point2.x - 1) * Math.Abs(largestDistance.point1.y - largestDistance.point2.y - 1);

long left = Math.Min(largestDistance.point1.x, largestDistance.point2.x);
long right = Math.Max(largestDistance.point1.x, largestDistance.point2.x);
long top = Math.Max(largestDistance.point1.y, largestDistance.point2.y);
long bottom = Math.Min(largestDistance.point1.y, largestDistance.point2.y);
long width = right - left + 1;
long height = top - bottom + 1;
Console.WriteLine(width * height);

static List<(double distance, (long x, long y) point1, (long x, long y) point2)> CalculateDistances(List<(long x, long y)> dataPoints)
{
    List<(double distance, (long x, long y) point1, (long x, long y) point2)> distances = new();

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

static double Distance2D(
    (long x, long y) point1,
    (long x, long y) point2)
{
    return Math.Sqrt(
        Math.Pow(point2.x - point1.x, 2) +
        Math.Pow(point2.y - point1.y, 2)
    );
}