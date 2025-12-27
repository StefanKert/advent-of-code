// Support both file input and WASM (stdin/env) input
string[] inputLines;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    inputLines = aocInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else if (Console.IsInputRedirected)
{
    inputLines = Console.In.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else
{
    inputLines = File.ReadAllLines("input.txt");
}

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(inputLines));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(inputLines));
}
else
{
    Console.WriteLine("Solution 1: Sum of invalid IDs: " + Solution1(inputLines));
    Console.WriteLine("Solution 2: Sum of invalid IDs: " + Solution2(inputLines));
}

static long Solution1(string[] inputLines)
{
    var ranges = inputLines
        .SelectMany(line => line.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .Select(part => part.Split('-').Select(long.Parse).ToArray())
        .Select(r => (Start: r[0], End: r[1]));

    var numbers = new List<string>();
    foreach (var line in ranges)
    {
        for (var i = line.Start; i <= line.End; i++)
        {
            numbers.Add(i.ToString());
        }
    }

    var invalidIds = new List<long>();
    for (int i = 0; i < numbers.Count; i++)
    {
        var lengthSquare = numbers[i].Length % 2;
        if (lengthSquare == 0)
        {
            var middle = numbers[i].Length / 2;
            var leftPart = numbers[i].Substring(0, middle);
            var rightPart = numbers[i].Substring(middle, middle);
            if (leftPart == rightPart)
            {
                invalidIds.Add(long.Parse(numbers[i]));
            }
        }
    }
    return invalidIds.Sum();
}

static long Solution2(string[] inputLines)
{
    var ranges = inputLines
        .SelectMany(line => line.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .Select(part => part.Split('-').Select(long.Parse).ToArray())
        .Select(r => (Start: r[0], End: r[1]));

    var numbers = new List<string>();
    foreach (var line in ranges)
    {
        for (var i = line.Start; i <= line.End; i++)
        {
            numbers.Add(i.ToString());
        }
    }

    var invalidIds = new List<long>();
    for (int i = 0; i < numbers.Count; i++)
    {
        if (IsPattern(numbers, i))
        {
            invalidIds.Add(long.Parse(numbers[i]));
        }
    }
    return invalidIds.Sum();
}

static bool IsPattern(List<string> numbers, int i)
{
    foreach (var combination in Combinations(numbers[i]))
    {
        if (IsOnlyPattern(numbers[i], combination))
        {
            return true;
        }
    }
    return false;
}

static List<string> Combinations(string input)
{
    var result = new List<string>();
    var temp = "";
    for (int i = 0; i < input.Length / 2; i++)
    {
        temp += input[i];
        result.Add(temp);
    }
    return result;
}

static bool IsOnlyPattern(string input, string pattern)
{
    for (int i = 0; i < input.Length; i++)
    {
        if (i + pattern.Length > input.Length) return false;
        var part = input.Substring(i, pattern.Length);
        if (part != pattern) return false;
        i += pattern.Length - 1;
    }
    return true;
}
