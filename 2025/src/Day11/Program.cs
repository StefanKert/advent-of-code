var lines = File.ReadAllLines("input.txt").ToDictionary(line => line.Split(':')[0], line => line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
var startLabel = lines.ContainsKey("svr") ? "svr" : lines.First(x => x.Value.Contains("you")).Value.First(x => x == "you");

var chains = WalkChain(lines, startLabel, false, false, new Dictionary<(string name, bool dac, bool fft), long>());
Console.WriteLine(chains);

static long WalkChain(Dictionary<string, List<string>> lines, string currentLabel, bool containsDac, bool containsFft,
        Dictionary<(string name, bool dac, bool fft), long> cache)
{
    if (!cache.ContainsKey((currentLabel, containsDac, containsFft)))
    {
        var currentChain = 0L;
        if (lines.ContainsKey(currentLabel))
        {
            if (currentLabel == "dac")
            {
                containsDac = true;
            }
            if (currentLabel == "fft")
            {
                containsFft = true;
            }

            if (lines[currentLabel].Contains("out"))
            {
                if (!containsDac || !containsFft)
                {
                    return 0;
                }
                return 1;
            }
            var nextLabels = lines[currentLabel];
            foreach (var item in nextLabels)
            {
                var chains = WalkChain(lines, item, containsDac, containsFft, cache);
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