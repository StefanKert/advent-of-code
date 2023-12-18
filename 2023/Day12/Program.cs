using System.Collections.Immutable;
using Cache = System.Collections.Generic.Dictionary<(string, System.Collections.Immutable.ImmutableStack<int>), long>;

var lines = await File.ReadAllLinesAsync("demo.txt");
var placements = 0L;
var repeat = 1;
foreach (var line in lines)
{
    var parts = line.Split(" ");
    var pattern = Unfold(parts[0], '?', repeat);
    var numString = Unfold(parts[1], ',', repeat);
    var nums = numString.Split(',').Select(int.Parse);
    placements += Compute(pattern, ImmutableStack.CreateRange(nums.Reverse()), new Cache());
}
Console.WriteLine(placements);

static string Unfold(string st, char join, int unfold) => string.Join(join, Enumerable.Repeat(st, unfold));

static long Compute(string pattern, ImmutableStack<int> nums, Cache cache)
{
    if (!cache.ContainsKey((pattern, nums)))
    {
        cache[(pattern, nums)] = Dispatch(pattern, nums, cache);
    }
    return cache[(pattern, nums)];
}

static long Dispatch(string pattern, ImmutableStack<int> nums, Cache cache) => pattern.FirstOrDefault() switch
{
    '.' => ProcessDot(pattern, nums, cache),
    '?' => ProcessQuestion(pattern, nums, cache),
    '#' => ProcessHash(pattern, nums, cache),
    _ => ProcessEnd(pattern, nums, cache),
};

static long ProcessEnd(string _, ImmutableStack<int> nums, Cache __) => nums.Any() ? 0 : 1;

static long ProcessDot(string pattern, ImmutableStack<int> nums, Cache cache) => Compute(pattern[1..], nums, cache);

static long ProcessQuestion(string pattern, ImmutableStack<int> nums, Cache cache) => Compute("." + pattern[1..], nums, cache) + Compute("#" + pattern[1..], nums, cache);

static long ProcessHash(string pattern, ImmutableStack<int> nums, Cache cache)
{
    if (!nums.Any())
    {
        return 0; 
    }

    var n = nums.Peek();
    nums = nums.Pop();

    var potentiallyDead = pattern.TakeWhile(s => s == '#' || s == '?').Count();
    if (potentiallyDead < n)
    {
        return 0; 
    }
    else if (pattern.Length == n)
    {
        return Compute("", nums, cache);
    }
    else if (pattern[n] == '#')
    {
        return 0; 
    }
    else
    {
        return Compute(pattern[(n + 1)..], nums, cache);
    }
}
