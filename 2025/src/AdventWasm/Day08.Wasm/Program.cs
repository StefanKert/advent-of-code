var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static List<(long x, long y, long z)> ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    return lines.Select(line =>
    {
        var parts = line.Split(',');
        return (long.Parse(parts[0].Trim()), long.Parse(parts[1].Trim()), long.Parse(parts[2].Trim()));
    }).ToList();
}

static double Distance3D((long x, long y, long z) p1, (long x, long y, long z) p2)
{
    return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2) + Math.Pow(p2.z - p1.z, 2));
}

static (List<int> clusterSizes, double lastDist) BuildClusters(List<(long x, long y, long z)> points)
{
    var distances = new List<(double dist, int i, int j)>();
    for (int i = 0; i < points.Count; i++)
        for (int j = i + 1; j < points.Count; j++)
            distances.Add((Distance3D(points[i], points[j]), i, j));
    distances.Sort((a, b) => a.dist.CompareTo(b.dist));

    var parent = Enumerable.Range(0, points.Count).ToArray();
    var rank = new int[points.Count];
    int Find(int x) { if (parent[x] != x) parent[x] = Find(parent[x]); return parent[x]; }
    bool Union(int x, int y)
    {
        var px = Find(x); var py = Find(y);
        if (px == py) return false;
        if (rank[px] < rank[py]) parent[px] = py;
        else if (rank[px] > rank[py]) parent[py] = px;
        else { parent[py] = px; rank[px]++; }
        return true;
    }

    int edgesUsed = 0; double lastDist = 0;
    foreach (var (dist, i, j) in distances)
    {
        if (Union(i, j)) { edgesUsed++; lastDist = dist; if (edgesUsed == points.Count - 1) break; }
    }

    var clusters = new Dictionary<int, int>();
    for (int i = 0; i < points.Count; i++)
    {
        var root = Find(i);
        clusters[root] = clusters.GetValueOrDefault(root) + 1;
    }
    return (clusters.Values.OrderByDescending(x => x).ToList(), lastDist);
}

static string SolvePart1(string input)
{
    var points = ParseInput(input);
    var (_, lastDist) = BuildClusters(points);
    return Math.Round(lastDist).ToString();
}

static string SolvePart2(string input)
{
    var points = ParseInput(input);
    var (clusterSizes, _) = BuildClusters(points);
    var product = clusterSizes.Take(3).Aggregate(1L, (a, b) => a * b);
    return product.ToString();
}
