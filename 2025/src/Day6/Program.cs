using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");


var matrix = new List<List<char>>();
for (int i = 0; i < lines.Length; i++)
{
    matrix.Add(new List<char>());
    foreach (var c in lines[i])
    {
        matrix[i].Add(c);
    }
}
Solution1(matrix);
Solution2(matrix);

static void Solution1(List<List<char>> chars)
{
    var sum = 0L;
    var lastRow = chars.Last();
    var i = 0;
    while (i < lastRow.Count)
    {
        var operation = lastRow[i];
        var currentOperatorIndex = i;
        var nextOperatorIndex = lastRow.FindIndex(i + 1, x => x == '+' || x == '*');
        if (nextOperatorIndex == -1)
        {
            nextOperatorIndex = lastRow.Count + 1;
        }

        var result = 0L;
        if (operation == '+')
        {
            result = chars.Take(chars.Count - 1).Sum(x => long.Parse(new string(x[i..(nextOperatorIndex - 1)].ToArray())));
        }
        if (operation == '*')
        {
            result = chars.Take(chars.Count - 1).Aggregate(1L, (acc, x) => acc * long.Parse(new string(x[i..(nextOperatorIndex - 1)].ToArray())));
        }
        sum += result;
        i = nextOperatorIndex;
    }
    Console.WriteLine($"Day 6: {sum}");
}

static void Solution2(List<List<char>> chars)
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

    var sum = 0L;
    foreach (var range in ranges)
    {
        var numbers = Enumerable.Range(range.start, range.end + 1 - range.start).Select(n => new string([.. chars[..^1].Select(x => x[n])]));
        if (range.operation == '+')
        {
            sum += numbers.Sum(x => long.Parse(x));
        }
        if(range.operation == '*')
        {
            sum += numbers.Aggregate(1L, (acc, x) => acc * long.Parse(x));
        }
    }
    Console.WriteLine($"Day 6: {sum}");
}