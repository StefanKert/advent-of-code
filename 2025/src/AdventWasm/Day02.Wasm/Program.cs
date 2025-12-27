var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static List<string> ParseInput(string input)
{
    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var numbers = new List<string>();
    foreach (var line in lines)
    {
        var ranges = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var range in ranges)
        {
            var parts = range.Trim().Split('-');
            if (parts.Length == 2 && long.TryParse(parts[0], out var start) && long.TryParse(parts[1], out var end))
            {
                for (var i = start; i <= end; i++) numbers.Add(i.ToString());
            }
        }
    }
    return numbers;
}

static bool IsPalindromicHalves(string number)
{
    if (number.Length % 2 != 0) return false;
    var middle = number.Length / 2;
    return number[..middle] == number[middle..];
}

static List<string> GetCombinations(string input)
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

static bool HasRepeatingPattern(string number)
{
    foreach (var combination in GetCombinations(number))
    {
        if (IsOnlyPattern(number, combination)) return true;
    }
    return false;
}

static string SolvePart1(string input)
{
    var numbers = ParseInput(input);
    var sum = 0L;
    foreach (var number in numbers)
    {
        if (IsPalindromicHalves(number)) sum += long.Parse(number);
    }
    return sum.ToString();
}

static string SolvePart2(string input)
{
    var numbers = ParseInput(input);
    var sum = 0L;
    foreach (var number in numbers)
    {
        if (HasRepeatingPattern(number)) sum += long.Parse(number);
    }
    return sum.ToString();
}
