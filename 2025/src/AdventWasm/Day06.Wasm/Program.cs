var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static List<List<char>> ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    return lines.Select(line => line.ToCharArray().ToList()).ToList();
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

static string SolvePart1(string input)
{
    var chars = ParseInput(input);
    if (chars.Count < 2) return "0";
    var ranges = GetRanges(chars);
    var dataRows = chars.Take(chars.Count - 1).ToList();
    var sum = ranges.Sum(range =>
    {
        var acc = range.operation == '+' ? 0L : 1L;
        foreach (var row in dataRows)
        {
            var segment = new string(row.Skip(range.start).Take(range.end - range.start + 1).ToArray());
            if (long.TryParse(segment.Replace(" ", ""), out var num))
                acc = range.operation == '+' ? acc + num : acc * num;
        }
        return acc;
    });
    return sum.ToString();
}

static string SolvePart2(string input)
{
    var chars = ParseInput(input);
    if (chars.Count < 2) return "0";
    var ranges = GetRanges(chars);
    var dataRows = chars.Take(chars.Count - 1).ToList();
    var sum = 0L;
    foreach (var range in ranges)
    {
        for (int col = range.start; col <= range.end && col < chars[0].Count; col++)
        {
            var acc = range.operation == '+' ? 0L : 1L;
            foreach (var row in dataRows)
            {
                if (col < row.Count && char.IsDigit(row[col]))
                {
                    var num = long.Parse(row[col].ToString());
                    acc = range.operation == '+' ? acc + num : acc * num;
                }
            }
            sum += acc;
        }
    }
    return sum.ToString();
}
