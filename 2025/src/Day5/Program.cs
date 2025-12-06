var inventory = ParseInventory("input.txt");

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

Solution1(inventory);
Solution2(inventory);

// Solution1(matrix);
// Solution2(matrix);


Inventory ParseInventory(string file)
{
    var data = File.ReadAllText(file).Split(Environment.NewLine + Environment.NewLine);
    var ranges = data[0].Split(Environment.NewLine).Select(x => new Range(fromInclusive: long.Parse(x.Split("-")[0]), toInclusive: long.Parse(x.Split("-")[1]))).OrderBy(x => x.fromInclusive).ToList();
    var ids = data[1].Split(Environment.NewLine).Select(long.Parse).ToList();
    Console.WriteLine($"Loaded {ranges.Count} ranges and {ids.Count} ids");
    while (MergeRanges(ranges)) { }
    Console.WriteLine($"Merged to {ranges.Count} ranges and {ids.Count} ids");
    return new Inventory(ranges, ids);
}

static void Solution1(Inventory inventory)
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
    Console.WriteLine(freshItems.Count);
}

static void Solution2(Inventory inventory)
{
    var counter = 0L;
    foreach (var range in inventory.freshIds.Where(x => x.fromInclusive > 0))
    {
        counter += range.toInclusive - range.fromInclusive + 1;
    }
    Console.WriteLine(counter);
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
