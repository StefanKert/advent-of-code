var lines = File.ReadAllLines("input.txt").ToDictionary(line => line.Split(':')[0], line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
var startLabel = lines.ContainsKey("svr") ? "svr" : lines.First(x => x.Value.Contains("you")).Value.First(x => x == "you");

var chains = WalkChain(lines, startLabel, [startLabel]);

Console.WriteLine(chains.Count);
Console.WriteLine(chains.Where(x => x.Contains("dac") && x.Contains("fft")).Count());

static List<List<string>> WalkChain(Dictionary<string, List<string>> lines, string currentLabel, List<string> chain)
{
    var currentChain = new List<List<string>>();
    if (lines.TryGetValue(currentLabel, out List<string>? nextLabels))
    {
        if (lines[currentLabel].Contains("out"))
        {
            return [[.. chain, lines[currentLabel].Single()]];
        }

        foreach (var item in nextLabels)
        {
            currentChain.AddRange(WalkChain(lines, item, [.. chain, item]));
        }
    }
    return currentChain;
}