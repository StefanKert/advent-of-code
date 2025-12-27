var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static (List<Range> ranges, List<long> ids) ParseInput(string input)
{
    var sections = input.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    var rangeLines = sections[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var ranges = rangeLines
        .Select(line => line.Split('-'))
        .Where(parts => parts.Length == 2)
        .Select(parts => new Range(long.Parse(parts[0].Trim()), long.Parse(parts[1].Trim())))
        .OrderBy(r => r.FromInclusive)
        .ToList();
    var ids = sections.Length > 1
        ? sections[1].Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => long.Parse(line.Trim())).ToList()
        : new List<long>();
    return (ranges, ids);
}

static bool MergeRanges(List<Range> ranges)
{
    for (int i = 1; i < ranges.Count; i++)
    {
        if (ranges[i].CompareTo(ranges[i - 1]) == 0) { ranges.RemoveAt(i); return true; }
        if (ranges[i].FromInclusive <= ranges[i - 1].ToInclusive && ranges[i].ToInclusive <= ranges[i - 1].ToInclusive)
        { ranges.RemoveAt(i); return true; }
        if (ranges[i].FromInclusive <= ranges[i - 1].ToInclusive)
        { ranges[i - 1].ToInclusive = ranges[i].ToInclusive; ranges.RemoveAt(i); return true; }
    }
    return false;
}

static string SolvePart1(string input)
{
    var (ranges, ids) = ParseInput(input);
    while (MergeRanges(ranges)) { }
    var count = ids.Count(id => ranges.Any(r => r.FromInclusive <= id && r.ToInclusive >= id));
    return count.ToString();
}

static string SolvePart2(string input)
{
    var (ranges, _) = ParseInput(input);
    while (MergeRanges(ranges)) { }
    var total = ranges.Where(r => r.FromInclusive > 0).Sum(r => r.ToInclusive - r.FromInclusive + 1);
    return total.ToString();
}

class Range : IComparable<Range>
{
    public long FromInclusive { get; set; }
    public long ToInclusive { get; set; }
    public Range(long from, long to) { FromInclusive = from; ToInclusive = to; }
    public int CompareTo(Range? other)
    {
        if (other == null) return 1;
        if (FromInclusive != other.FromInclusive) return FromInclusive.CompareTo(other.FromInclusive);
        return ToInclusive.CompareTo(other.ToInclusive);
    }
}
