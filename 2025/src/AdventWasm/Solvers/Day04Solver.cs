namespace AdventWasm.Solvers;

public class Day04Solver : ISolver
{
    public string Title => "Paper Roll Detection";
    public string Description => "Count paper roll locations based on neighbor density (< 4 neighbors).";

    private static readonly (int dx, int dy)[] Directions = new[]
    {
        (-1, -1), (-1, 0), (-1, 1),
        (0, -1),           (0, 1),
        (1, -1),  (1, 0),  (1, 1)
    };

    private static List<List<char>> ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.Select(line => line.ToCharArray().ToList()).ToList();
    }

    private static int CountNeighbors(List<List<char>> matrix, int x, int y, char target)
    {
        var count = 0;
        foreach (var (dx, dy) in Directions)
        {
            var newX = x + dx;
            var newY = y + dy;
            if (newX >= 0 && newX < matrix.Count && newY >= 0 && newY < matrix[0].Count)
            {
                if (matrix[newX][newY] == target) count++;
            }
        }
        return count;
    }

    public string SolvePart1(string input)
    {
        var matrix = ParseInput(input);
        var count = 0;

        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == '@')
                {
                    var neighbors = CountNeighbors(matrix, i, j, '@');
                    if (neighbors < 4) count++;
                }
            }
        }

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        var matrix = ParseInput(input);
        var totalRemoved = 0;

        bool changed;
        do
        {
            changed = false;
            for (var i = 0; i < matrix.Count; i++)
            {
                for (var j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] == '@')
                    {
                        var neighbors = CountNeighbors(matrix, i, j, '@');
                        if (neighbors < 4)
                        {
                            matrix[i][j] = '.';
                            totalRemoved++;
                            changed = true;
                        }
                    }
                }
            }
        } while (changed);

        return totalRemoved.ToString();
    }
}
