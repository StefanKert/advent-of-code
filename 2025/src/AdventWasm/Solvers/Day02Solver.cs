namespace AdventWasm.Solvers;

public class Day02Solver : ISolver
{
    public string Title => "Invalid ID Detection";
    public string Description => "Validate numbers based on palindromic halves and repeating patterns.";

    private static List<string> ParseInput(string input)
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
                    for (var i = start; i <= end; i++)
                    {
                        numbers.Add(i.ToString());
                    }
                }
            }
        }

        return numbers;
    }

    private static bool IsPalindromicHalves(string number)
    {
        if (number.Length % 2 != 0) return false;
        var middle = number.Length / 2;
        return number[..middle] == number[middle..];
    }

    private static List<string> GetCombinations(string input)
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

    private static bool IsOnlyPattern(string input, string pattern)
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

    private static bool HasRepeatingPattern(string number)
    {
        foreach (var combination in GetCombinations(number))
        {
            if (IsOnlyPattern(number, combination)) return true;
        }
        return false;
    }

    public string SolvePart1(string input)
    {
        var numbers = ParseInput(input);
        var sum = 0L;

        foreach (var number in numbers)
        {
            if (IsPalindromicHalves(number))
            {
                sum += long.Parse(number);
            }
        }

        return sum.ToString();
    }

    public string SolvePart2(string input)
    {
        var numbers = ParseInput(input);
        var sum = 0L;

        foreach (var number in numbers)
        {
            if (HasRepeatingPattern(number))
            {
                sum += long.Parse(number);
            }
        }

        return sum.ToString();
    }
}
