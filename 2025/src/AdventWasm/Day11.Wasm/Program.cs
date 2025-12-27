var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static Dictionary<string, List<string>> ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    return lines.ToDictionary(
        line => line.Split(':')[0].Trim(),
        line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
    );
}

static long WalkChain(Dictionary<string, List<string>> lines, string currentLabel,
    bool containsDac, bool containsFft, Dictionary<(string, bool, bool), long> cache, bool filter = false)
{
    if (!cache.ContainsKey((currentLabel, containsDac, containsFft)))
    {
        var currentChain = 0L;
        if (lines.ContainsKey(currentLabel))
        {
            if (filter && currentLabel == "dac") containsDac = true;
            if (filter && currentLabel == "fft") containsFft = true;
            if (lines[currentLabel].Contains("out"))
            {
                if (filter && (!containsDac || !containsFft)) return 0;
                return 1;
            }
            foreach (var item in lines[currentLabel])
            {
                var chains = WalkChain(lines, item, containsDac, containsFft, cache, filter);
                if (chains > 0) currentChain += chains;
            }
        }
        cache[(currentLabel, containsDac, containsFft)] = currentChain;
    }
    return cache[(currentLabel, containsDac, containsFft)];
}

static string SolvePart1(string input)
{
    var lines = ParseInput(input);
    var startEntry = lines.FirstOrDefault(x => x.Value.Contains("you"));
    if (startEntry.Key == null) return "0";
    var startLabel = startEntry.Value.First(x => x == "you");
    var cache = new Dictionary<(string, bool, bool), long>();
    var result = WalkChain(lines, startLabel, false, false, cache, false);
    return result.ToString();
}

static string SolvePart2(string input)
{
    var lines = ParseInput(input);
    var cache = new Dictionary<(string, bool, bool), long>();
    var result = WalkChain(lines, "svr", false, false, cache, true);
    return result.ToString();
}
