var rows = File.ReadAllLines("input.txt").Select(Row.Parse).ToList();
Console.WriteLine($"Part 1: Safe rows count {rows.Count(x => x.IsSafe())}");
Console.WriteLine($"Part 2: Safe rows count {rows.Count(x => x.IsSafeWithDampener())}");

public record Row(List<int> reports)
{
    public static Row Parse(string line)
    {
        var parts = line.Split(" ").Select(int.Parse).ToList();
        return new Row(parts);
    }

    public bool IsSafe()
    {
        var decreaseing = Direction.Undefined;
        for (int i = 0; i < reports.Count - 1; i++)
        {
            var diffCols = reports[i] - reports[i + 1];
            if (Math.Abs(diffCols) > 3 || Math.Abs(diffCols) < 1)
            {
                return false;
            }

            if (diffCols < 0)
            {
                if (decreaseing == Direction.Decreasing)
                {
                    return false;
                }
                decreaseing = Direction.Increasing;
            }
            else
            {
                if (decreaseing == Direction.Increasing)
                {
                    return false;
                }
                decreaseing = Direction.Decreasing;
            }
        }
        return true;
    }

    public bool IsSafeWithDampener()
    {
        if(IsSafe())
        {
            return true;
        }

        for (int i = 0; i < reports.Count; i++)
        {
            var copy = reports.ToList();
            copy.RemoveAt(i);
            if (new Row(copy).IsSafe())
            {
                return true;
            }
        }
        return false;
    }
}


public enum Direction
{
    Undefined,
    Decreasing,
    Increasing
}