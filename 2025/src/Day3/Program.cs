// Support both file input and WASM (stdin/env) input
string inputText;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    inputText = aocInput;
}
else if (Console.IsInputRedirected)
{
    inputText = Console.In.ReadToEnd();
}
else
{
    inputText = File.ReadAllText("input.txt");
}

var rawData = GetRawData(inputText);

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(rawData.Sum(bank => Aggregate(bank, 2)));
}
else if (partStr == "2")
{
    Console.WriteLine(rawData.Sum(bank => Aggregate(bank, 12)));
}
else
{
    Console.WriteLine("Solution 1: " + rawData.Sum(bank => Aggregate(bank, 2)));
    Console.WriteLine("Solution 2: " + rawData.Sum(bank => Aggregate(bank, 12)));
}

static long Aggregate(List<long> batteries, int batteriesEnabled) => batteriesEnabled == 0 ? 0 : (batteries[0..^(batteriesEnabled - 1)].Max() * (long)Math.Pow(10, (batteriesEnabled - 1))) + Aggregate(batteries[(batteries.IndexOf(batteries[0..^(batteriesEnabled - 1)].Max()) + 1)..], (batteriesEnabled - 1));

static List<List<long>> GetRawData(string text) => text.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Select(y => long.Parse(y.ToString())).ToList()).ToList();
