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

var inventory = ParseInventory(inputText);

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(inventory));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(inventory));
}
else
{
    Console.WriteLine("Solution 1: " + Solution1(inventory));
    Console.WriteLine("Solution 2: " + Solution2(inventory));
}

static bool MergeRanges(List<Range> ranges)
{
    for (int i = 1; i < ranges.Count; i++)
    {
        if (ranges[i].CompareTo(ranges[i - 1]) == 0)
        {
            ranges.RemoveAt(i);
            return true;
        }

        if (ranges[i].fromInclusive <= ranges[i - 1].toInclusive && ranges[i].toInclusive <= ranges[i - 1].toInclusive)
        {
            ranges.RemoveAt(i);
            return true;
        }

        if (ranges[i].fromInclusive <= ranges[i - 1].toInclusive)
        {
            ranges[i - 1].toInclusive = ranges[i].toInclusive;
            ranges.Remove(ranges[i]);
            return true;
        }
    }
    return false;
}

Inventory ParseInventory(string text)
{
    var data = text.Replace("\r\n", "\n").Split("\n\n");
    var ranges = data[0].Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => new Range(fromInclusive: long.Parse(x.Split("-")[0]), toInclusive: long.Parse(x.Split("-")[1]))).OrderBy(x => x.fromInclusive).ToList();
    var ids = data[1].Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
    while (MergeRanges(ranges)) { }
    return new Inventory(ranges, ids);
}

static long Solution1(Inventory inventory)
{
    var freshItems = new List<long>();
    foreach (var number in inventory.ids)
    {
        var validRanges = inventory.freshIds.Where(range => range.fromInclusive <= number && range.toInclusive >= number);
        if (validRanges.Any())
        {
            freshItems.Add(number);
        }
    }
    return freshItems.Count;
}

static long Solution2(Inventory inventory)
{
    var counter = 0L;
    foreach (var range in inventory.freshIds.Where(x => x.fromInclusive > 0))
    {
        counter += range.toInclusive - range.fromInclusive + 1;
    }
    return counter;
}

public record Inventory(List<Range> freshIds, List<long> ids);

public class Range : IComparable<Range>
{

    public long fromInclusive { get; set; }
    public long toInclusive { get; set; }

    public Range(long fromInclusive, long toInclusive)
    {
        this.fromInclusive = fromInclusive;
        this.toInclusive = toInclusive;
    }

    public int CompareTo(Range? other)
    {
        if (other == null) return 1;
        if (this.fromInclusive != other.fromInclusive)
        {
            return this.fromInclusive.CompareTo(other.fromInclusive);
        }
        return this.toInclusive.CompareTo(other.toInclusive);
    }
}
