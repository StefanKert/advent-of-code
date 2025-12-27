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

var matrix = lines.Select(line => new List<char>(line)).ToList();

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(matrix));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(matrix));
}
else
{
    Console.WriteLine("Solution 1: " + Solution1(matrix));
    Console.WriteLine("Solution 2: " + Solution2(matrix));
}

static long Solution1(List<List<char>> chars)
{
    return GetRanges(chars).Sum(range => chars[..^1].Select(x => new string([.. x[range.start..(range.end + 1)]])).Aggregate(range.operation == '+' ? 0L : 1L, (acc, x) => range.operation == '+' ? acc + long.Parse(x) : acc * long.Parse(x)));
}

static long Solution2(List<List<char>> chars)
{
    return GetRanges(chars).Sum(range => Enumerable.Range(range.start, range.end + 1 - range.start).Select(n => new string([.. chars[..^1].Select(x => x[n])])).Aggregate(range.operation == '+' ? 0L : 1L, (acc, x) => range.operation == '+' ? acc + long.Parse(x) : acc * long.Parse(x)));
}

static List<(int start, int end, char operation)> GetRanges(List<List<char>> chars)
{
    var operatorRow = chars.Last();

    var ranges = new List<(int start, int end, char operation)>();
    var startRange = 0;
    var endRange = 0;
    var currentOperator = operatorRow[0];
    for (int i = 1; i < operatorRow.Count - 2; i++)
    {
        endRange = i;
        if (operatorRow[i + 2] == '+' || operatorRow[i + 2] == '*')
        {
            ranges.Add((startRange, endRange, currentOperator));
            i += 2;
            startRange = i;
            currentOperator = operatorRow[i];
        }
    }
    ranges.Add((startRange, operatorRow.Count - 1, currentOperator));
    return ranges;
}
