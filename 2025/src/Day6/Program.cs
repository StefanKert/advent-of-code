var matrix = File.ReadAllLines("input.txt").Select(line => new List<char>(line)).ToList();

Solution1(matrix);
Solution2(matrix);

static void Solution1(List<List<char>> chars)
{
    var sum = GetRanges(chars).Sum(range => chars[..^1].Select(x => new string([.. x[range.start..(range.end + 1)]])).Aggregate(range.operation == '+' ? 0L : 1L, (acc, x) => range.operation == '+' ? acc + long.Parse(x) : acc * long.Parse(x)));
    Console.WriteLine($"Day 6: {sum}");
}

static void Solution2(List<List<char>> chars)
{
    var sum = GetRanges(chars).Sum(range => Enumerable.Range(range.start, range.end + 1 - range.start).Select(n => new string([.. chars[..^1].Select(x => x[n])])).Aggregate(range.operation == '+' ? 0L : 1L, (acc, x) => range.operation == '+' ? acc + long.Parse(x) : acc * long.Parse(x)));
    Console.WriteLine($"Day 6: {sum}");
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