namespace AdventWasm.Solvers;

public class Day12Solver : ISolver
{
    public string Title => "Shape Packing";
    public string Description => "Check if shapes fit in regions based on occupied squares.";

    private static (List<List<string>> shapes, List<(long width, long height, List<long> shapeCounts)> regions) ParseInput(string input)
    {
        var lines = input.Split('\n').ToList();
        var lastIndex = lines.LastIndexOf(string.Empty);
        if (lastIndex < 0) lastIndex = lines.FindLastIndex(s => string.IsNullOrWhiteSpace(s));

        var shapeRows = lastIndex >= 0 ? lines.Take(lastIndex).ToList() : new List<string>();
        var shapes = new List<List<string>>();

        for (int i = 0; i < shapeRows.Count; i++)
        {
            var row = shapeRows[i];
            if (string.IsNullOrWhiteSpace(row))
            {
                continue;
            }
            if (i + 3 < shapeRows.Count)
            {
                shapes.Add(new List<string>
                {
                    shapeRows[i + 1],
                    shapeRows[i + 2],
                    shapeRows[i + 3]
                });
                i += 3;
            }
        }

        var regionLines = lastIndex >= 0 && lastIndex + 1 < lines.Count
            ? lines.Skip(lastIndex + 1).Where(x => !string.IsNullOrWhiteSpace(x)).ToList()
            : new List<string>();

        var regions = regionLines.Select(x =>
        {
            var parts = x.Split(':');
            var dims = parts[0].Split('x');
            var shapeCounts = parts.Length > 1
                ? parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()
                : new List<long>();
            return (width: long.Parse(dims[0]), height: long.Parse(dims[1]), shapeCounts);
        }).ToList();

        return (shapes, regions);
    }

    public string SolvePart1(string input)
    {
        var (_, regions) = ParseInput(input);
        var count = 0;

        foreach (var region in regions)
        {
            var area = region.width * region.height;
            var shapeCounts = region.shapeCounts;

            // Calculate occupied squares based on shape sizes
            // Shapes 0-3 occupy 7 squares, shape 4 occupies 6, shape 5 occupies 5
            long occupiedSquares = 0;
            if (shapeCounts.Count > 0) occupiedSquares += 7 * shapeCounts[0];
            if (shapeCounts.Count > 1) occupiedSquares += 7 * shapeCounts[1];
            if (shapeCounts.Count > 2) occupiedSquares += 7 * shapeCounts[2];
            if (shapeCounts.Count > 3) occupiedSquares += 7 * shapeCounts[3];
            if (shapeCounts.Count > 4) occupiedSquares += 6 * shapeCounts[4];
            if (shapeCounts.Count > 5) occupiedSquares += 5 * shapeCounts[5];

            if (area > occupiedSquares)
                count++;
        }

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        var (_, regions) = ParseInput(input);
        long totalExcess = 0;

        foreach (var region in regions)
        {
            var area = region.width * region.height;
            var shapeCounts = region.shapeCounts;

            long occupiedSquares = 0;
            if (shapeCounts.Count > 0) occupiedSquares += 7 * shapeCounts[0];
            if (shapeCounts.Count > 1) occupiedSquares += 7 * shapeCounts[1];
            if (shapeCounts.Count > 2) occupiedSquares += 7 * shapeCounts[2];
            if (shapeCounts.Count > 3) occupiedSquares += 7 * shapeCounts[3];
            if (shapeCounts.Count > 4) occupiedSquares += 6 * shapeCounts[4];
            if (shapeCounts.Count > 5) occupiedSquares += 5 * shapeCounts[5];

            if (area > occupiedSquares)
                totalExcess += (area - occupiedSquares);
        }

        return totalExcess.ToString();
    }
}
