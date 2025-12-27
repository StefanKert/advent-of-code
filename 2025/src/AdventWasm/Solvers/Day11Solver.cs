namespace AdventWasm.Solvers;

public class Day11Solver : ISolver
{
    public string Title => "Dependency Chain Traversal";
    public string Description => "Walk dependency chains and count paths to 'out' with optional filtering.";

    private static Dictionary<string, List<string>> ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.ToDictionary(
            line => line.Split(':')[0].Trim(),
            line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
        );
    }

    private static long WalkChain(
        Dictionary<string, List<string>> lines,
        string currentLabel,
        bool containsDac,
        bool containsFft,
        Dictionary<(string name, bool dac, bool fft), long> cache,
        bool filter = false)
    {
        if (!cache.ContainsKey((currentLabel, containsDac, containsFft)))
        {
            var currentChain = 0L;
            if (lines.ContainsKey(currentLabel))
            {
                if (filter && currentLabel == "dac")
                {
                    containsDac = true;
                }
                if (filter && currentLabel == "fft")
                {
                    containsFft = true;
                }

                if (lines[currentLabel].Contains("out"))
                {
                    if (filter && (!containsDac || !containsFft))
                    {
                        return 0;
                    }
                    return 1;
                }
                var nextLabels = lines[currentLabel];
                foreach (var item in nextLabels)
                {
                    var chains = WalkChain(lines, item, containsDac, containsFft, cache, filter);
                    if (chains > 0)
                    {
                        currentChain += chains;
                    }
                }
            }
            cache[(currentLabel, containsDac, containsFft)] = currentChain;
        }
        return cache[(currentLabel, containsDac, containsFft)];
    }

    public string SolvePart1(string input)
    {
        var lines = ParseInput(input);
        var startEntry = lines.FirstOrDefault(x => x.Value.Contains("you"));
        if (startEntry.Key == null) return "0";

        var startLabel = startEntry.Value.First(x => x == "you");
        var cache = new Dictionary<(string name, bool dac, bool fft), long>();
        var result = WalkChain(lines, startLabel, false, false, cache, false);
        return result.ToString();
    }

    public string SolvePart2(string input)
    {
        var lines = ParseInput(input);
        var cache = new Dictionary<(string name, bool dac, bool fft), long>();
        var result = WalkChain(lines, "svr", false, false, cache, true);
        return result.ToString();
    }
}
