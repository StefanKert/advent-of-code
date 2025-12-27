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

var junctionBoxes = lines.Select(x => (x: long.Parse(x.Split(",")[0]), y: long.Parse(x.Split(",")[1]), z: long.Parse(x.Split(",")[2]))).ToList();
var distances = CalculateDistances(junctionBoxes);

(double distance, (long x, long y, long z) point1, (long x, long y, long z) point2) lastPoint = (0, (0, 0, 0), (0, 0, 0));
var circuits = new List<List<(long x, long y, long z)>>();
foreach (var point in distances.OrderBy(x => x.distance))
{
    if (circuits.Any(x => x.Contains(point.point1) && x.Contains(point.point2)))
    {
        continue;
    }

    var tempLastPoint = lastPoint;
    if (junctionBoxes.Count + circuits.Count == 2)
    {
        lastPoint = point;
    }

    if (circuits.Any(x => x.Contains(point.point1)))
    {
        if (circuits.Any(x => x.Contains(point.point2)))
        {
            junctionBoxes.Remove(point.point2);
            var junctionBoxesMErge = circuits.First(x => x.Contains(point.point2));
            circuits.Remove(junctionBoxesMErge);
            circuits.First(x => x.Contains(point.point1)).AddRange(junctionBoxesMErge);
            continue;
        }
        else
        {
            junctionBoxes.Remove(point.point2);
            circuits.First(x => x.Contains(point.point1)).Add(point.point2);
            continue;
        }
    }
    else if (circuits.Any(x => x.Contains(point.point2)))
    {
        if (circuits.Any(x => x.Contains(point.point1)))
        {
            junctionBoxes.Remove(point.point1);
            var junctionBoxesMErge = circuits.First(x => x.Contains(point.point1));
            circuits.Remove(junctionBoxesMErge);
            circuits.First(x => x.Contains(point.point2)).AddRange(junctionBoxesMErge);
            continue;
        }
        else
        {
            junctionBoxes.Remove(point.point1);
            circuits.First(x => x.Contains(point.point2)).Add(point.point1);
            continue;
        }
    }
    else
    {
        junctionBoxes.Remove(point.point1);
        junctionBoxes.Remove(point.point2);
        circuits.Add(new List<(long x, long y, long z)>() { point.point1, point.point2 });
    }

    if (junctionBoxes.Count == 0 && circuits.Count == 1)
    {
        lastPoint = tempLastPoint;
    }
}

var result = lastPoint.point1.x * lastPoint.point2.x;

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(result);
}
else if (partStr == "2")
{
    Console.WriteLine(result);
}
else
{
    Console.WriteLine("Solution: " + result);
}

static double Distance3D(
    (long x, long y, long z) point1,
    (long x, long y, long z) point2)
{
    return Math.Sqrt(
        Math.Pow(point2.x - point1.x, 2) +
        Math.Pow(point2.y - point1.y, 2) +
        Math.Pow(point2.z - point1.z, 2)
    );
}

static List<(double distance, (long x, long y, long z) point1, (long x, long y, long z) point2)> CalculateDistances(List<(long x, long y, long z)> junctionBoxes)
{
    List<(double distance, (long x, long y, long z) point1, (long x, long y, long z) point2)> distances = new();

    var seen = new HashSet<(int, int)>();

    for (int i = 0; i < junctionBoxes.Count; i++)
    {
        for (int j = 0; j < junctionBoxes.Count; j++)
        {
            if (i == j) continue;

            var a = Math.Min(i, j);
            var b = Math.Max(i, j);

            if (!seen.Add((a, b))) continue;

            var d = Distance3D(junctionBoxes[i], junctionBoxes[j]);
            distances.Add((d, junctionBoxes[i], junctionBoxes[j]));
        }
    }

    return distances;
}
