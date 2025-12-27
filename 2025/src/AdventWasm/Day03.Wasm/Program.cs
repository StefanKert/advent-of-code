var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static List<List<long>> ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    return lines.Select(line => line.Select(c => long.Parse(c.ToString())).ToList()).ToList();
}

static long Aggregate(List<long> batteries, int batteriesEnabled)
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

static string SolvePart1(string input)
{
    var grid = ParseInput(input);
    var total = grid.Sum(row => Aggregate(row, 2));
    return total.ToString();
}

static string SolvePart2(string input)
{
    var grid = ParseInput(input);
    var total = grid.Sum(row => Aggregate(row, 12));
    return total.ToString();
}
