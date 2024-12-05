var (updates, comparer) = Parse(File.ReadAllText("input.txt"));

Console.WriteLine($"Part 1:  {updates.Where(pages => Sorted(pages, comparer)).Sum(GetMiddlePage)}");
Console.WriteLine($"Part 2: {updates.Where(pages => !Sorted(pages, comparer)).Select(pages => pages.OrderBy(p => p, comparer).ToArray()).Sum(GetMiddlePage)}");

static (string[][] updates, Comparer<string>) Parse(string input)
{
    var parts = input.Split(Environment.NewLine + Environment.NewLine);
    var ordering = new HashSet<string>(parts[0].Split(Environment.NewLine));
    var comparer = Comparer<string>.Create((p1, p2) => ordering.Contains(p1 + "|" + p2) ? -1 : 1);

    var updates = parts[1].Split(Environment.NewLine).Select(line => line.Split(",")).ToArray();
    return (updates, comparer);
}

static int GetMiddlePage(string[] nums) => int.Parse(nums[nums.Length / 2]);
static bool Sorted(string[] pages, Comparer<string> comparer) => Enumerable.SequenceEqual(pages, pages.OrderBy(x => x, comparer));