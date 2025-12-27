// Support both file input and WASM (stdin/env) input
string[] lines;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    lines = aocInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else if (Console.IsInputRedirected)
{
    lines = Console.In.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else
{
    lines = File.ReadAllLines("input.txt");
}

var coordinates = lines
    .Select(x => {
        var parts = x.Split(',');
        return (x: long.Parse(parts[0]), y: long.Parse(parts[1]));
    })
    .ToList();

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(coordinates));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(coordinates));
}
else
{
    Console.WriteLine("Part 1: " + Solution1(coordinates));
    Console.WriteLine("Part 2: " + Solution2(coordinates));
}

static long Solution1(List<(long x, long y)> coordinates)
{
    long maxArea1 = 0;
    for (int i = 0; i < coordinates.Count; i++)
    {
        for (int j = i + 1; j < coordinates.Count; j++)
        {
            var p1 = coordinates[i];
            var p2 = coordinates[j];
            long width = Math.Abs(p1.x - p2.x) + 1;
            long height = Math.Abs(p1.y - p2.y) + 1;
            maxArea1 = Math.Max(maxArea1, width * height);
        }
    }
    return maxArea1;
}

static long Solution2(List<(long x, long y)> coordinates)
{
    var edges = new List<((long x, long y) from, (long x, long y) to)>();
    for (int i = 0; i < coordinates.Count; i++)
    {
        int next = (i + 1) % coordinates.Count;
        edges.Add((coordinates[i], coordinates[next]));
    }

    long maxArea2 = 0;
    for (int i = 0; i < coordinates.Count; i++)
    {
        for (int j = i + 1; j < coordinates.Count; j++)
        {
            var p1 = coordinates[i];
            var p2 = coordinates[j];

            long left = Math.Min(p1.x, p2.x);
            long right = Math.Max(p1.x, p2.x);
            long bottom = Math.Min(p1.y, p2.y);
            long top = Math.Max(p1.y, p2.y);

            if (IsRectangleInside(left, right, bottom, top, coordinates, edges))
            {
                long width = right - left + 1;
                long height = top - bottom + 1;
                maxArea2 = Math.Max(maxArea2, width * height);
            }
        }
    }
    return maxArea2;
}

static bool IsRectangleInside(long left, long right, long bottom, long top,
                               List<(long x, long y)> polygon,
                               List<((long x, long y) from, (long x, long y) to)> edges)
{
    foreach (var edge in edges)
    {
        if (EdgeCrossesRectangleInterior(edge, left, right, bottom, top))
            return false;
    }

    double centerX = (left + right) / 2.0 + 0.5;
    double centerY = (bottom + top) / 2.0 + 0.5;

    return IsPointInPolygon(centerX, centerY, polygon);
}

static bool EdgeCrossesRectangleInterior(((long x, long y) from, (long x, long y) to) edge,
                                          long left, long right, long bottom, long top)
{
    var (from, to) = edge;

    if (from.y == to.y)
    {
        long y = from.y;
        long x1 = Math.Min(from.x, to.x);
        long x2 = Math.Max(from.x, to.x);

        return y > bottom && y < top && x1 < right && x2 > left;
    }
    else
    {
        long x = from.x;
        long y1 = Math.Min(from.y, to.y);
        long y2 = Math.Max(from.y, to.y);

        return x > left && x < right && y1 < top && y2 > bottom;
    }
}

static bool IsPointInPolygon(double x, double y, List<(long x, long y)> polygon)
{
    bool inside = false;
    int n = polygon.Count;

    for (int i = 0, j = n - 1; i < n; j = i++)
    {
        var pi = polygon[i];
        var pj = polygon[j];

        if (((pi.y > y) != (pj.y > y)) &&
            (x < (double)(pj.x - pi.x) * (y - pi.y) / (pj.y - pi.y) + pi.x))
        {
            inside = !inside;
        }
    }

    return inside;
}
