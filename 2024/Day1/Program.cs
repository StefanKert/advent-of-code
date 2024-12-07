using AdventOfCode.Helpers;

var solution = new SolutionDay1();  
solution.PrintSolutions();

public class SolutionDay1 : AbstractSolution
{
    public override long Part1()
    {
        var lines = _input.Split(Environment.NewLine);
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in lines)
        {
            var parts = line.Split("   ");
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }
        leftList.Sort();
        rightList.Sort();
        var totalDistance = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            totalDistance = totalDistance + Math.Abs(leftList[i] - rightList[i]);
        }
        return totalDistance;
    }

    public override long Part2()
    {
        var lines = _input.Split(Environment.NewLine);
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in lines)
        {
            var parts = line.Split("   ");
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }
        leftList.Sort();
        rightList.Sort();
        var similarityScore = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            var amounts = rightList.Count(x => x == leftList[i]);
            similarityScore = leftList[i] * amounts + similarityScore;
        }
        return similarityScore;
    }
}