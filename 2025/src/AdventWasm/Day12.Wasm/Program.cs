var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 0 ? args[0] : "1");
if (!int.TryParse(partStr, out int part)) part = 1;

var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input)) input = Console.In.ReadToEnd();

var result = part == 1 ? SolvePart1(input) : SolvePart2(input);
Console.WriteLine(result);

static (List<List<(int x, int y)>> shapes, List<(int width, int height, int[] quantities)> regions) ParseInput(string input)
{
    var lines = input.Split('\n').ToList();
    var shapes = new List<List<(int x, int y)>>();
    var regions = new List<(int width, int height, int[] quantities)>();

    int i = 0;
    while (i < lines.Count)
    {
        var line = lines[i];
        if (line.Contains('x') && line.Contains(':')) break;
        if (line.EndsWith(':') && int.TryParse(line.TrimEnd(':'), out _))
        {
            var shapeLines = new List<string>();
            i++;
            while (i < lines.Count && !string.IsNullOrEmpty(lines[i]) && !lines[i].Contains(':'))
            {
                shapeLines.Add(lines[i]);
                i++;
            }
            var cells = new List<(int x, int y)>();
            for (int y = 0; y < shapeLines.Count; y++)
                for (int x = 0; x < shapeLines[y].Length; x++)
                    if (shapeLines[y][x] == '#') cells.Add((x, y));
            shapes.Add(cells);
        }
        else i++;
    }

    while (i < lines.Count)
    {
        var line = lines[i];
        if (!string.IsNullOrWhiteSpace(line) && line.Contains('x') && line.Contains(':'))
        {
            var parts = line.Split(':');
            var dims = parts[0].Split('x');
            int width = int.Parse(dims[0]);
            int height = int.Parse(dims[1]);
            var quantities = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            regions.Add((width, height, quantities));
        }
        i++;
    }
    return (shapes, regions);
}

static List<(int x, int y)>[] GetAllOrientations(List<(int x, int y)> cells)
{
    var seen = new HashSet<string>();
    var result = new List<List<(int x, int y)>>();
    var current = cells.Select(c => (c.x, c.y)).ToList();
    for (int r = 0; r < 4; r++)
    {
        AddIfNew(current, seen, result);
        var flipped = current.Select(c => (-c.x, c.y)).ToList();
        AddIfNew(flipped, seen, result);
        current = current.Select(c => (c.y, -c.x)).ToList();
    }
    return result.ToArray();
}

static void AddIfNew(List<(int x, int y)> cells, HashSet<string> seen, List<List<(int x, int y)>> result)
{
    int minX = cells.Min(c => c.x);
    int minY = cells.Min(c => c.y);
    var normalized = cells.Select(c => (c.x - minX, c.y - minY)).OrderBy(c => c.Item1).ThenBy(c => c.Item2).ToList();
    string key = string.Join(";", normalized.Select(c => $"{c.Item1},{c.Item2}"));
    if (seen.Add(key)) result.Add(normalized);
}

static bool CanFitAllPresents(int width, int height, int[] quantities, List<(int x, int y)>[][] allOrientations, int[] shapeCellCounts)
{
    int totalCellsNeeded = 0;
    for (int s = 0; s < quantities.Length; s++) totalCellsNeeded += quantities[s] * shapeCellCounts[s];
    if (totalCellsNeeded > width * height) return false;

    var piecesToPlace = new List<int>();
    for (int s = 0; s < quantities.Length; s++)
        for (int c = 0; c < quantities[s]; c++) piecesToPlace.Add(s);
    piecesToPlace = piecesToPlace.OrderByDescending(s => shapeCellCounts[s]).ToList();

    var grid = new bool[width, height];
    return TryPlace(grid, width, height, piecesToPlace, 0, allOrientations, shapeCellCounts, totalCellsNeeded);
}

static bool TryPlace(bool[,] grid, int width, int height, List<int> pieces, int pieceIdx,
              List<(int x, int y)>[][] allOrientations, int[] shapeCellCounts, int remainingCells)
{
    if (pieceIdx >= pieces.Count) return true;
    int emptyCount = 0;
    for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            if (!grid[x, y]) emptyCount++;
    if (emptyCount < remainingCells) return false;

    int shapeIdx = pieces[pieceIdx];
    foreach (var orientation in allOrientations[shapeIdx])
    {
        int maxX = orientation.Max(c => c.x);
        int maxY = orientation.Max(c => c.y);
        for (int py = 0; py <= height - maxY - 1; py++)
        {
            for (int px = 0; px <= width - maxX - 1; px++)
            {
                if (CanPlaceAt(grid, width, height, orientation, px, py))
                {
                    PlaceShape(grid, orientation, px, py, true);
                    if (TryPlace(grid, width, height, pieces, pieceIdx + 1, allOrientations, shapeCellCounts, remainingCells - shapeCellCounts[shapeIdx]))
                        return true;
                    PlaceShape(grid, orientation, px, py, false);
                }
            }
        }
    }
    return false;
}

static bool CanPlaceAt(bool[,] grid, int width, int height, List<(int x, int y)> shape, int px, int py)
{
    foreach (var (x, y) in shape)
    {
        int gx = px + x, gy = py + y;
        if (gx < 0 || gx >= width || gy < 0 || gy >= height || grid[gx, gy]) return false;
    }
    return true;
}

static void PlaceShape(bool[,] grid, List<(int x, int y)> shape, int px, int py, bool place)
{
    foreach (var (x, y) in shape) grid[px + x, py + y] = place;
}

static string SolvePart1(string input)
{
    var (shapes, regions) = ParseInput(input);
    var allOrientations = shapes.Select(s => GetAllOrientations(s)).ToArray();
    var shapeCellCounts = shapes.Select(s => s.Count).ToArray();

    int count = 0;
    foreach (var region in regions)
    {
        if (CanFitAllPresents(region.width, region.height, region.quantities, allOrientations, shapeCellCounts))
            count++;
    }
    return count.ToString();
}

static string SolvePart2(string input)
{
    // Part 2 logic would go here - for now return 0
    return "0";
}
