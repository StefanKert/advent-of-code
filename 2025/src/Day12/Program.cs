// Support both file input and WASM (stdin/env) input
string[] lines;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    lines = aocInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else if (Console.IsInputRedirected)
{
    lines = Console.In.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else
{
    var inputFile = args.Length > 0 ? args[0] : "input.txt";
    lines = File.ReadAllLines(inputFile);
}

var input = lines.ToList();
var (shapes, regions) = ParseInput(input);

var allOrientations = new List<List<(int x, int y)>[]>();
var shapeCellCounts = new List<int>();

foreach (var shape in shapes)
{
    var orientations = GetAllOrientations(shape.Cells);
    allOrientations.Add(orientations);
    shapeCellCounts.Add(shape.Cells.Count);
}

int count = 0;
object lockObj = new object();

Parallel.For(0, regions.Count, r =>
{
    var region = regions[r];
    if (CanFitAllPresents(region.width, region.height, region.quantities,
                          allOrientations.ToArray(), shapeCellCounts.ToArray()))
    {
        lock (lockObj) { count++; }
    }
});

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(count);
}
else if (partStr == "2")
{
    Console.WriteLine(count);
}
else
{
    Console.WriteLine($"Part 1: {count}");
}

(List<Shape> shapes, List<Region> regions) ParseInput(List<string> lines)
{
    var shapes = new List<Shape>();
    var regions = new List<Region>();

    int i = 0;
    while (i < lines.Count)
    {
        var line = lines[i];
        if (line.Contains('x') && line.Contains(':'))
            break;

        if (line.EndsWith(':') && int.TryParse(line.TrimEnd(':'), out int shapeIdx))
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
            {
                for (int x = 0; x < shapeLines[y].Length; x++)
                {
                    if (shapeLines[y][x] == '#')
                        cells.Add((x, y));
                }
            }
            shapes.Add(new Shape(shapeIdx, cells));
        }
        else
        {
            i++;
        }
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
            var quantities = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            regions.Add(new Region(width, height, quantities));
        }
        i++;
    }

    return (shapes, regions);
}

List<(int x, int y)>[] GetAllOrientations(List<(int x, int y)> cells)
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

void AddIfNew(List<(int x, int y)> cells, HashSet<string> seen, List<List<(int x, int y)>> result)
{
    int minX = cells.Min(c => c.x);
    int minY = cells.Min(c => c.y);
    var normalized = cells.Select(c => (c.x - minX, c.y - minY)).OrderBy(c => c.Item1).ThenBy(c => c.Item2).ToList();

    string key = string.Join(";", normalized.Select(c => $"{c.Item1},{c.Item2}"));
    if (seen.Add(key))
    {
        result.Add(normalized);
    }
}

bool CanFitAllPresents(int width, int height, int[] quantities,
                       List<(int x, int y)>[][] allOrientations, int[] shapeCellCounts)
{
    int totalCellsNeeded = 0;
    for (int s = 0; s < quantities.Length; s++)
        totalCellsNeeded += quantities[s] * shapeCellCounts[s];

    if (totalCellsNeeded > width * height)
        return false;

    var piecesToPlace = new List<int>();
    for (int s = 0; s < quantities.Length; s++)
    {
        for (int c = 0; c < quantities[s]; c++)
            piecesToPlace.Add(s);
    }

    piecesToPlace = piecesToPlace.OrderByDescending(s => shapeCellCounts[s]).ToList();

    var grid = new bool[width, height];
    return TryPlace(grid, width, height, piecesToPlace, 0, allOrientations, shapeCellCounts, totalCellsNeeded);
}

bool TryPlace(bool[,] grid, int width, int height, List<int> pieces, int pieceIdx,
              List<(int x, int y)>[][] allOrientations, int[] shapeCellCounts, int remainingCells)
{
    if (pieceIdx >= pieces.Count)
        return true;

    int emptyCount = 0;
    for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            if (!grid[x, y]) emptyCount++;

    if (emptyCount < remainingCells)
        return false;

    int shapeIdx = pieces[pieceIdx];
    var orientations = allOrientations[shapeIdx];

    foreach (var orientation in orientations)
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

                    if (TryPlace(grid, width, height, pieces, pieceIdx + 1, allOrientations,
                                 shapeCellCounts, remainingCells - shapeCellCounts[shapeIdx]))
                    {
                        return true;
                    }

                    PlaceShape(grid, orientation, px, py, false);
                }
            }
        }
    }

    return false;
}

bool CanPlaceAt(bool[,] grid, int width, int height, List<(int x, int y)> shape, int px, int py)
{
    foreach (var (x, y) in shape)
    {
        int gx = px + x;
        int gy = py + y;
        if (gx < 0 || gx >= width || gy < 0 || gy >= height)
            return false;
        if (grid[gx, gy])
            return false;
    }
    return true;
}

void PlaceShape(bool[,] grid, List<(int x, int y)> shape, int px, int py, bool place)
{
    foreach (var (x, y) in shape)
    {
        grid[px + x, py + y] = place;
    }
}

record Shape(int Index, List<(int x, int y)> Cells);
record Region(int width, int height, int[] quantities);
