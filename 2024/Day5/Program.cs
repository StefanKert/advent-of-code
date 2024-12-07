using AdventOfCode.Helpers;

var solutionDay5 = new SolutionDay5();
solutionDay5.PrintSolutions();

public class SolutionDay5 : AbstractSolution
{
    public SolutionDay5() : base() { }
    public SolutionDay5(string filepath) : base(filepath) { }

    public override long Part1()
    {
        var (updates, comparer) = Parse(_input);
        return updates.Where(pages => Sorted(pages, comparer)).Sum(GetMiddlePage);
    }
    public override long Part2()
    {
        var (updates, comparer) = Parse(_input);
        return updates.Where(pages => !Sorted(pages, comparer)).Select(pages => pages.OrderBy(p => p, comparer).ToArray()).Sum(GetMiddlePage);
    }

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
}