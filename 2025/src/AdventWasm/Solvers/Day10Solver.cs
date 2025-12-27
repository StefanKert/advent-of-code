namespace AdventWasm.Solvers;

public class Day10Solver : ISolver
{
    public string Title => "Beam Tree Traversal";
    public string Description => "Walk beam paths through a map, counting splits and distinct paths.";

    private static (int height, int width, List<char> map, int startingPoint) ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var height = lines.Length;
        var width = lines[0].Length;
        var map = lines.SelectMany(line => line.ToCharArray()).ToList();
        var startingPoint = map.IndexOf('S');
        return (height, width, map, startingPoint);
    }

    private static int Index(int x, int y, int width) => y * width + x;

    private static bool InBounds(int x, int y, int width, int height) =>
        x >= 0 && y >= 0 && x < width && y < height;

    public string SolvePart1(string input)
    {
        var (height, width, map, startingPoint) = ParseInput(input);
        var currentRow = 1;
        var beams = new List<int> { startingPoint };
        var splitCounter = 0;

        while (currentRow + 1 < height)
        {
            currentRow++;
            var nextBeams = new List<int>();
            foreach (var beam in beams)
            {
                var idx = Index(beam, currentRow, width);
                if (idx >= 0 && idx < map.Count && map[idx] == '^')
                {
                    if (InBounds(beam - 1, currentRow, width, height))
                    {
                        nextBeams.Add(beam - 1);
                    }
                    if (InBounds(beam + 1, currentRow, width, height))
                    {
                        nextBeams.Add(beam + 1);
                    }
                    splitCounter++;
                }
                else
                {
                    nextBeams.Add(beam);
                }
            }
            beams = nextBeams.Where(b => b >= 0 && b < width).ToList();
        }

        return splitCounter.ToString();
    }

    public string SolvePart2(string input)
    {
        var (height, width, map, startingPoint) = ParseInput(input);

        long WalkTree(List<char> currentMap, int x, int currentRow)
        {
            var distinctCount = 0L;
            while (currentRow + 1 < height)
            {
                currentRow++;
                var idx = Index(x, currentRow, width);
                if (idx >= 0 && idx < currentMap.Count && currentMap[idx] == '^')
                {
                    if (InBounds(x - 1, currentRow, width, height))
                    {
                        distinctCount += WalkTree(new List<char>(currentMap), x - 1, currentRow - 1);
                    }
                    if (InBounds(x + 1, currentRow, width, height))
                    {
                        distinctCount += WalkTree(new List<char>(currentMap), x + 1, currentRow - 1);
                    }
                    return distinctCount;
                }
            }
            return distinctCount + 1;
        }

        var result = WalkTree(new List<char>(map), startingPoint, 1);
        return result.ToString();
    }
}
