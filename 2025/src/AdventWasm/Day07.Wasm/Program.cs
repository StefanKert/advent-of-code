var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static (int height, int width, List<char> map, int startingPoint) ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var height = lines.Length;
    var width = lines[0].Length;
    var map = lines.SelectMany(line => line.ToCharArray()).ToList();
    var startingPoint = map.IndexOf('S');
    return (height, width, map, startingPoint);
}

static string SolvePart1(string input)
{
    var (height, width, map, startingPoint) = ParseInput(input);
    var beams = new List<int> { startingPoint };
    var splitCounter = 0;
    for (int row = 1; row < height; row++)
    {
        var nextBeams = new List<int>();
        foreach (var beam in beams)
        {
            var idx = row * width + beam;
            if (idx >= 0 && idx < map.Count && map[idx] == '^')
            {
                splitCounter++;
                if (beam - 1 >= 0) nextBeams.Add(beam - 1);
                if (beam + 1 < width) nextBeams.Add(beam + 1);
            }
            else nextBeams.Add(beam);
        }
        beams = nextBeams.Where(b => b >= 0 && b < width).ToList();
    }
    return splitCounter.ToString();
}

static string SolvePart2(string input)
{
    var (height, width, map, startingPoint) = ParseInput(input);
    var memo = new Dictionary<(int x, int row), long>();

    long WalkCount(int x, int row)
    {
        if (memo.TryGetValue((x, row), out var cached)) return cached;
        int r = row;
        while (r + 1 < height)
        {
            r++;
            var idx = r * width + x;
            if (idx >= 0 && idx < map.Count && map[idx] == '^')
            {
                long sum = 0;
                if (x - 1 >= 0) sum += WalkCount(x - 1, r - 1);
                if (x + 1 < width) sum += WalkCount(x + 1, r - 1);
                return memo[(x, row)] = sum;
            }
        }
        return memo[(x, row)] = 1;
    }

    var result = WalkCount(startingPoint, 1);
    return result.ToString();
}
