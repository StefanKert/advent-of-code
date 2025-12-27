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

var linesDict = lines.ToDictionary(line => line.Split(':')[0], line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    var chains = WalkChain(linesDict, linesDict.First(x => x.Value.Contains("you")).Value.First(x => x == "you"), false, false, new Dictionary<(string name, bool dac, bool fft), long>());
    Console.WriteLine(chains);
}
else if (partStr == "2")
{
    var chains = WalkChain(linesDict, "svr", false, false, new Dictionary<(string name, bool dac, bool fft), long>(), true);
    Console.WriteLine(chains);
}
else
{
    var chains = WalkChain(linesDict, linesDict.First(x => x.Value.Contains("you")).Value.First(x => x == "you"), false, false, new Dictionary<(string name, bool dac, bool fft), long>());
    Console.WriteLine("Solution 1: " + chains);

    chains = WalkChain(linesDict, "svr", false, false, new Dictionary<(string name, bool dac, bool fft), long>(), true);
    Console.WriteLine("Solution 2: " + chains);
}

static long WalkChain(Dictionary<string, List<string>> lines, string currentLabel, bool containsDac, bool containsFft,
        Dictionary<(string name, bool dac, bool fft), long> cache, bool filter = false)
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
